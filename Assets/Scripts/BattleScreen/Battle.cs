using System.Collections;
using System.Collections.Generic;
using BattleScene;
using BattleScreen.BattleEvents;
using Disturbances;
using UnityEngine;

namespace BattleScreen
{
    public enum GameState
    {
        Loading,
        Paused,
        PlayerTurn,
        ExecutingPlayerTurnEvents,
        EnemyTurn,
        Rewards
    }
    
    public class Battle : MonoBehaviour
    {
        public const float ActionExecutionSpeed = 0.6f;
        
        public static Battle Current;
        
        
        [SerializeField] private EndOfBattlePopup _endOfBattlePopup;
        [SerializeField] private BattleHUDManager _hudManager;

        public int Turn { get; private set; } = 1;
        public GameState GameState { get; private set; } = GameState.Loading;
        
        private void Awake()
        {
            if (Current)
                Destroy(Current.gameObject);

            Current = this;
        }

        private void Start()
        {
            foreach (var design in RunProgress.PlayerStats.Deck)
            {
                var plank = PlankFactory.Current.CreatePlank();
                EtchingFactory.Current.CreateEtching(plank, design);
            }
            
            // Give the game one frame to load etchings, enemies etc
            StartCoroutine(InitializeBattle());
        }

        public void ClickedNextTurn()
        {
            if (GameState != GameState.PlayerTurn) return;

            StartCoroutine(Turn < RunProgress.PlayerStats.NumberOfTurns ? NextEnemyTurn() : EndBattle());
        }
        
        public void ClickedQuit()
        {
            throw new System.NotImplementedException();
        }

        public void InvokeResponsesToPlayerTurnEvent(BattleEvent battleEvent)
        {
            StartCoroutine(ExecutePlayerTurnEvents(battleEvent));
        }

        private IEnumerator InitializeBattle()
        {
            Debug.Log("------ Starting battle ------");
            yield return 0;
            yield return 0;

            var startBattle = new BattleEvent(BattleEventType.StartedBattle);
            yield return StartCoroutine(StartChainOfEvents(startBattle));
            
            SetToPlayersTurn();
        }

        private IEnumerator ExecutePlayerTurnEvents(BattleEvent battleEvent)
        {
            GameState = GameState.ExecutingPlayerTurnEvents;
            yield return StartCoroutine(StartChainOfEvents(battleEvent));
            GameState = GameState.PlayerTurn;
        }
        
        private IEnumerator NextEnemyTurn()
        {
            GameState = GameState.EnemyTurn;
            
            Debug.Log("------ Starting Enemy Turn ------");
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.StartedNextTurn)));
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.StartedEnemyMovementPeriod)));
            Debug.Log("------ Ending Enemy Turn ------");
            yield return new WaitForSecondsRealtime(ActionExecutionSpeed);
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.EndedEnemyTurn)));
            Turn++;
            SetToPlayersTurn();
        }

        private IEnumerator EndBattle()
        {
            Debug.Log("------ Ending Battle ------");
            yield return StartCoroutine(StartChainOfEvents(new BattleEvent(BattleEventType.EndedBattle)));
            GameState = GameState.Rewards;
            CreateEndOfBattlePopup();
        }

        private IEnumerator StartChainOfEvents(BattleEvent battleEvent)
        {
            BattleRenderer.Current.RenderEventPackage(new BattleEventPackage(battleEvent));
            yield return StartCoroutine(Tick(battleEvent));
        }
        
        private IEnumerator Tick(BattleEvent previousBattleEvent)
        {
            //Debug.Log("Now responding to : " + previousBattleEvent.type);

            for (var i = 0; i < 1000; i++)
            {
                var response = BattleEventsManager.Current.GetNextResponse(previousBattleEvent);
                if (response.IsEmpty) break;
                BattleRenderer.Current.RenderEventPackage(response);

                foreach (var battleEvent in response.battleEvents)
                {
                    var waitTime = GetAnimationTime(battleEvent.type);
                    if (waitTime > 0f)
                        yield return new WaitForSecondsRealtime(waitTime * ActionExecutionSpeed);
                    
                    yield return StartCoroutine(Tick(battleEvent));
                    Debug.Log("Finished " + battleEvent.type + ". Back to " + previousBattleEvent.type);
                }
            }
        }

        private float GetAnimationTime(BattleEventType type)
        {
            switch (type)
            {
                case BattleEventType.EnemyAboutToMove:
                case BattleEventType.EtchingActivated:
                    return 1f;
                case BattleEventType.EnemyStartOfTurnEffect:
                    return 0.5f;
            }

            return 0f;
        }

        private void SetToPlayersTurn()
        {
            GameState = GameState.PlayerTurn;
            Debug.Log("------ PLAYERS TURN! ------");
        }

        private void CreateEndOfBattlePopup()
        {
            _endOfBattlePopup.gameObject.SetActive(true);
            var disturbance = RunProgress.CurrentDisturbance;
            _endOfBattlePopup.SetReward(disturbance);
            _endOfBattlePopup.SetButtonAction(LeaveBattle);
        
        }

        private void LeaveBattle()
        {
            DisturbanceManager.ExecuteEndOfBattleDisturbanceAction(RunProgress.CurrentDisturbance);
            RunProgress.PlayerStats.Gold = Player.Current.Gold;
            RunProgress.PlayerStats.Lives = Player.Current.Lives;
            MainManager.Current.LoadOfferScreen();
            Destroy(gameObject);
        }
    }
}