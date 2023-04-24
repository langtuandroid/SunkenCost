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
        EnemyTurn,
        Rewards
    }
    
    public class Battle : MonoBehaviour
    {
        public const float ActionExecutionSpeed = 0.3f;
        
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

            if (Turn < RunProgress.PlayerStats.NumberOfTurns) StartCoroutine(NextEnemyTurn());
            else StartCoroutine(EndBattle());
        }
        
        public void ClickedQuit()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator VisualiseBattleEvents(Queue<BattleEvent> battleEvents)
        {
            Debug.Log("------ Visualising ------");
            while (battleEvents.Count > 0)
            {
                var nextBattleEvent = battleEvents.Dequeue();
                
                Debug.Log(nextBattleEvent.type);
                _hudManager.UpdateDisplay(nextBattleEvent.type);

                foreach (var visualiser in nextBattleEvent.visualisers)
                {
                    Debug.Log(visualiser + " loading state");
                    visualiser.LoadNextState();
                }

                if (nextBattleEvent.type == BattleEventType.None)
                    continue;
                
                if (nextBattleEvent.type == BattleEventType.PlayerDied)
                    yield break;
                
                if (nextBattleEvent.type == BattleEventType.EnemyDamaged)
                    yield return new WaitForSeconds
                        (ActionExecutionSpeed * nextBattleEvent.damageModificationPackage.ModCount);

                var waitTime = nextBattleEvent.type switch
                {
                    BattleEventType.EnemyMove => ActionExecutionSpeed / 3,
                    _ => ActionExecutionSpeed
                };
                yield return new WaitForSeconds(waitTime);
            }
        }
        
        private IEnumerator InitializeBattle()
        {
            Debug.Log("------ Starting battle ------");
            yield return 0;
            var startBattleEvents = BattleEventsManager.Current.StartBattle();
            yield return StartCoroutine(VisualiseBattleEvents(new Queue<BattleEvent>(startBattleEvents)));
            SetToPlayersTurn();
        }

        private IEnumerator NextEnemyTurn()
        {
            GameState = GameState.EnemyTurn;
            
            Debug.Log("------ Processing Next Enemy Turn ------");
            var battleActions = BattleEventsManager.Current.GetNextTurn();
            var battleActionQueue = new Queue<BattleEvent>(battleActions);
            yield return StartCoroutine(VisualiseBattleEvents(battleActionQueue));
            Turn++;
            SetToPlayersTurn();
        }

        private IEnumerator EndBattle()
        {
            var endBattleEvents = BattleEventsManager.Current.EndBattle();
            yield return StartCoroutine(VisualiseBattleEvents(new Queue<BattleEvent>(endBattleEvents)));
            GameState = GameState.Rewards;
            CreateEndOfBattlePopup();
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