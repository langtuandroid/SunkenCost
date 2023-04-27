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
            yield return StartCoroutine(Tick(startBattle));
            
            SetToPlayersTurn();
        }

        private IEnumerator ExecutePlayerTurnEvents(BattleEvent battleEvent)
        {
            GameState = GameState.ExecutingPlayerTurnEvents;
            yield return StartCoroutine(Tick(battleEvent));
            GameState = GameState.PlayerTurn;
        }
        
        private IEnumerator NextEnemyTurn()
        {
            GameState = GameState.EnemyTurn;
            
            Debug.Log("------ Starting Enemy Turn ------");
            yield return StartCoroutine(Tick(new BattleEvent(BattleEventType.StartedNextTurn)));
            yield return StartCoroutine(Tick(new BattleEvent(BattleEventType.StartedEnemyMovementPeriod)));
            Debug.Log("------ Ending Enemy Turn ------");
            yield return StartCoroutine(Tick(new BattleEvent(BattleEventType.EndedEnemyTurn)));
            Turn++;
            SetToPlayersTurn();
        }

        private IEnumerator EndBattle()
        {
            Debug.Log("------ Ending Battle ------");
            yield return StartCoroutine(Tick(new BattleEvent(BattleEventType.EndedBattle)));
            GameState = GameState.Rewards;
            CreateEndOfBattlePopup();
        }
        
        private IEnumerator Tick(BattleEvent previousBattleEvent)
        {
            //Debug.Log("Now responding to : " + previousBattleEvent.type);

            for (var i = 0; i < 1000; i++)
            {
                var response = BattleEventsManager.Current.GetNextResponse(previousBattleEvent);
                if (response.IsEmpty) yield break;
                BattleRenderer.Current.RenderEventPackage(response);

                foreach (var battleEvent in response.battleEvents)
                {
                    if (battleEvent.type == BattleEventType.EnemyAboutToMove ||
                        battleEvent.type == BattleEventType.EnemyMove ||
                        battleEvent.type == BattleEventType.EtchingActivated)
                        yield return new WaitForSecondsRealtime(ActionExecutionSpeed);
                    
                    yield return StartCoroutine(Tick(battleEvent));
                    Debug.Log("Finished " + battleEvent.type + ". Back to " + previousBattleEvent.type);
                }
            }
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