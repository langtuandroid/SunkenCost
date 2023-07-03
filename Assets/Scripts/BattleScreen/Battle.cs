using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScene;
using BattleScreen.BattleEvents;
using BattleScreen.UI;
using Damage;
using Disturbances;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleScreen
{
    public enum BattleState
    {
        Loading,
        Paused,
        PlayerActionPeriod,
        ExecutingPlayerTurnEvents,
        EnemyTurn,
        Rewards,
        PlayerDied
    }

    public enum BattleSpeed
    {
        Normal,
        Fast,
        Ultra,
    }
    
    public class Battle : MonoBehaviour
    {
        public static float ActionExecutionSpeed { get; private set; }
        
        public static Battle Current;

        [SerializeField] private EndOfBattlePopup _endOfBattlePopup;
        [SerializeField] private PlayerDeathPopup _playerDeathPopup;
        [SerializeField] private PlayerDeathPopup _winPopup;
        [FormerlySerializedAs("_hudManager")] [SerializeField] private BattleHUD hud;
        
        private InGameSfxManager _sfxManager;
        private BattleRenderer _battleRenderer;

        public int Turn { get; private set; } = 0;
        public BattleState BattleState { get; private set; } = BattleState.Loading;
        
        private void Awake()
        {
            if (Current)
                Destroy(Current.gameObject);

            Current = this;
        }

        private void Start()
        {
            _sfxManager = InGameSfxManager.current;
            _battleRenderer = BattleRenderer.Current;
            
            foreach (var design in RunProgress.Current.PlayerStats.Deck)
            {
                var plank = PlankFactory.Current.CreatePlank();
                EtchingFactory.Current.CreateEtching(plank, design);
            }

            // Give the game one frame to load etchings, enemies etc
            StartCoroutine(InitializeBattle());
        }

        public void ClickedNextTurn()
        {
            if (BattleState != BattleState.PlayerActionPeriod) return;
            StartCoroutine(NextEnemyTurn());
        }
        
        public void ClickedQuit()
        {
            throw new System.NotImplementedException();
        }

        public void InvokeResponsesToPlayerTurnEvent(BattleEvent battleEvent)
        {
            StartCoroutine(ExecutePlayerTurnEvents(battleEvent));
        }

        public void SetBattleSpeed(BattleSpeed battleSpeed)
        {
            Settings.BattleSpeed = battleSpeed;
            
            ActionExecutionSpeed = battleSpeed switch
            {
                BattleSpeed.Normal => 0.8f,
                BattleSpeed.Fast => 0.6f,
                BattleSpeed.Ultra => 0.3f,
                _ => throw new ArgumentOutOfRangeException(nameof(battleSpeed), battleSpeed, null)
            };
        }

        private IEnumerator InitializeBattle()
        {
            Debug.Log("------ Starting battle ------");
            // Gives other initialisers a clear frame to initialise
            yield return 0;

            yield return 0;

            var startBattle = new BattleEvent(BattleEventType.StartedBattle);
            BattleEventResponseSequencer.Current.RefreshTransforms();
            yield return StartCoroutine(StartChainOfEvents(startBattle));
            yield return StartCoroutine(NextPlayerTurn());
        }
        
        private IEnumerator NextPlayerTurn()
        {
            Turn++;
            if (Turn <= RunProgress.Current.PlayerStats.NumberOfTurns)
            {
                Debug.Log("------ PLAYERS TURN! ------");
                yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.StartedNextPlayerTurn)));
                BattleState = BattleState.PlayerActionPeriod;
            }
            else
            {
                yield return StartCoroutine(EndBattle());
            }
        }

        private IEnumerator ExecutePlayerTurnEvents(BattleEvent battleEvent)
        {
            BattleState = BattleState.ExecutingPlayerTurnEvents;
            
            // Give the board time to refresh if the player moved a plank
            if (battleEvent.type == BattleEventType.PlayerMovedPlank)
            {
                yield return 0;
            }

            yield return StartCoroutine(StartChainOfEvents(battleEvent));
            BattleState = BattleState.PlayerActionPeriod;
        }
        
        private IEnumerator NextEnemyTurn()
        {
            BattleState = BattleState.EnemyTurn;
            Debug.Log("------ Starting Enemy Turn ------");
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.StartedEnemyTurn)));
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.StartedEnemyMovementPeriod)));
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.EndedEnemyTurn)));
            Debug.Log("------ Ending Enemy Turn ------");
            yield return StartCoroutine(NextPlayerTurn());
        }

        private IEnumerator EndBattle()
        {
            Debug.Log("------ Ending Battle ------");
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.EndedBattle)));
            BattleState = BattleState.Rewards;
            yield return new WaitForSecondsRealtime(ActionExecutionSpeed / 3f);
            CreateEndOfBattlePopup();
        }

        private IEnumerator StartChainOfEvents(BattleEvent battleEvent)
        {
            ExecuteAudioVisualCues(new BattleEventPackage(battleEvent));

            var sequenceOfResponses = Tick(battleEvent);
            while (sequenceOfResponses.MoveNext()) 
            {
                yield return sequenceOfResponses.Current;
            }
        }

        private void ExecuteAudioVisualCues(BattleEventPackage battleEventPackage)
        {
            _battleRenderer.RenderEventPackage(battleEventPackage);
            _sfxManager.TriggerAudio(battleEventPackage);
        }
        
        private IEnumerator Tick(BattleEvent previousBattleEvent)
        {
            if (previousBattleEvent.type == BattleEventType.PlayerDied)
            {
                BattleState = BattleState.PlayerDied;
                _playerDeathPopup.gameObject.SetActive(true);
                StopAllCoroutines();
            }

            var hasHadAnyResponse = false;
            
            while (true)
            {
                var responsePackage = BattleEventResponseSequencer.Current.GetNextResponse(previousBattleEvent);
                if (responsePackage.IsEmpty) break;

                hasHadAnyResponse = true;

                ExecuteAudioVisualCues(responsePackage);

                // Remove events that were only used for rendering
                var battleEventsList = responsePackage.battleEvents.Where
                (b => !((b.type == BattleEventType.EtchingActivated || b.type == BattleEventType.ItemActivated) 
                    && !b.showAsOwnAction)).ToList();
                
                // Sometimes we need a f rame for transforms to change parents etc.
                if (BattleEventListContainsTransformUpdatingEvent(battleEventsList))
                {
                    Debug.Log("Waiting for transforms...");
                    yield return 0;
                    yield return 0;
                    Debug.Log("Transforms refreshed!");
                    BattleEventResponseSequencer.Current.RefreshTransforms();
                }

                var battleEventsQueue = new Queue<BattleEvent>(battleEventsList);
                
                while (battleEventsQueue.Count > 0)
                {
                    var battleEvent = battleEventsQueue.Dequeue();
                    
                    var waitTime = -1f;
                    
                    // Only add wait time to the last in a batch event
                    if (BattleState == BattleState.EnemyTurn && battleEventsQueue.Count(b => b.type == battleEvent.type) == 0)
                        waitTime = GetAnimationTime(battleEvent);
                    else
                    {
                        Debug.Log("Multiple " + battleEvent.type);
                    }

                    if (waitTime > 0f)
                    {
                        Debug.Log($"Waiting {waitTime*ActionExecutionSpeed} for {battleEvent.type}");
                        yield return new WaitForSecondsRealtime(waitTime * ActionExecutionSpeed);
                    }

                    var sequenceOfResponses = Tick(battleEvent);
                    while (sequenceOfResponses.MoveNext()) 
                    {
                        yield return sequenceOfResponses.Current;
                    }
                    
                    Debug.Log("Finished " + battleEvent.type + ". Back to " + previousBattleEvent.type);
                }
            }

            if (hasHadAnyResponse) yield break;
            
            // Only wait if no etching, enemy or item have done anything since the last move - this stops
            // the enemy from jumping straight to the next plank
            if (previousBattleEvent.type == BattleEventType.EnemyMoved)
            {
                yield return new WaitForSecondsRealtime(ActionExecutionSpeed);
            }
        }

        public static float GetAnimationTime(BattleEvent battleEvent)
        {
            var type = battleEvent.type;
            
            switch (type)
            {
                case BattleEventType.PlayerLostLife:
                    return 1.3f;
                case BattleEventType.EtchingActivated when battleEvent.showAsOwnAction:
                    return 1.5f;
                case BattleEventType.StartedIndividualEnemyTurn:
                case BattleEventType.EnemyEffect:
                case BattleEventType.EnemyMaxHealthModified:
                    return 1f;
                case BattleEventType.EnemySpawned when battleEvent.showAsOwnAction:
                case BattleEventType.PlayerGainedLife:
                case BattleEventType.EnemyKilled when battleEvent.source != DamageSource.Boat:
                    return 0.75f;
                case BattleEventType.EnemyBlocked:
                case BattleEventType.ItemActivated when battleEvent.showAsOwnAction:
                    return 0.5f;
                case BattleEventType.EnemyMoved:
                    return 0.5f;
            }

            return -1f;
        }

        private void CreateEndOfBattlePopup()
        {
            if (RunProgress.Current.BattleNumber >= 10)
            {
                _winPopup.gameObject.SetActive(true);
                return;
            }

            _endOfBattlePopup.gameObject.SetActive(true);
            var disturbance = RunProgress.Current.CurrentDisturbance;
            _endOfBattlePopup.SetReward(disturbance);
            _endOfBattlePopup.SetButtonAction(LeaveBattle);
        
        }

        private void LeaveBattle()
        {
            RunProgress.Current.PlayerStats.Health = Player.Current.Health;
            DisturbanceLoader.ExecuteEndOfBattleDisturbanceAction(RunProgress.Current.CurrentDisturbance);
            MainManager.Current.LoadOfferScreen();
            Destroy(gameObject);
        }

        private static bool BattleEventListContainsTransformUpdatingEvent(List<BattleEvent> battleEvents)
        {
            return battleEvents.Any(b => 
                b.type == BattleEventType.StartedBattle ||
                b.type == BattleEventType.StartedNextPlayerTurn ||
                b.type == BattleEventType.EtchingsOrderChanged ||
                b.type == BattleEventType.PlankCreated ||
                b.type == BattleEventType.PlankMoved ||
                b.type == BattleEventType.PlankDestroyed);
        }
    }
}