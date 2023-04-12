using System.Collections;
using System.Collections.Generic;
using BattleScene;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;
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

        public int Turn { get; private set; }
        public GameState GameState { get; private set; } = GameState.Loading;
        
        private void Awake()
        {
            if (Current)
                Destroy(Current.gameObject);

            Current = this;
        }

        private void Start()
        {
            BattleEvents.BattleEventsManager.Current.StartBattle();
        }

        public void ClickedNextTurn()
        {
            if (GameState == GameState.PlayerTurn) StartCoroutine(NextEnemyTurn());
        }

        public void VisualisePlayerEvents(Queue<BattleEvent> battleEvents)
        {
            
        }

        private IEnumerator NextEnemyTurn()
        {
            Turn++;
            GameState = GameState.EnemyTurn;
            var battleActions = BattleEventsManager.Current.GetNextTurn();
            var battleActionQueue = new Queue<BattleEvent>(battleActions);
            yield return StartCoroutine(VisualiseBattleEvents(battleActionQueue));
            GameState = GameState.PlayerTurn;
        }

        public static IEnumerator VisualiseBattleEvents(Queue<BattleEvent> battleEvents)
        {
            while (battleEvents.Count > 0)
            {
                var nextBattleEvent = battleEvents.Dequeue();
                
                if (nextBattleEvent.battleEventType == BattleEventType.None)
                    continue;
                
                if (nextBattleEvent.battleEventType == BattleEventType.PlayerDied)
                    yield break;

                if (nextBattleEvent is VisualisedBattleEvent v)
                {
                    v.visualiser.StartVisualisationCoroutine(nextBattleEvent);
                }
                
                yield return new WaitForSeconds(ActionExecutionSpeed);
            }
        }

        private void CreateEndOfBattlePopup()
        {
            _endOfBattlePopup.gameObject.SetActive(true);
            var disturbance = RunProgress.CurrentDisturbance;
            _endOfBattlePopup.SetReward(disturbance);
            _endOfBattlePopup.SetButtonAction(EndBattle);
        
        }

        private void EndBattle()
        {
            DisturbanceManager.ExecuteEndOfBattleDisturbanceAction(RunProgress.CurrentDisturbance);
            RunProgress.PlayerStats.Gold = Player.Current.Gold;
            RunProgress.PlayerStats.Lives = Player.Current.Lives;
            MainManager.Current.LoadOfferScreen();
            Destroy(gameObject);
        }
    }
}