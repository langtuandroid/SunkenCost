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
            foreach (var design in RunProgress.PlayerStats.Deck)
            {
                var plank = PlankMap.Current.CreatePlank();
                EtchingMap.Current.CreateEtching(plank, design);
            }
            
            BattleEventsManager.Current.StartBattle();
        }

        public void ClickedNextTurn()
        {
            if (GameState == GameState.PlayerTurn) StartCoroutine(NextEnemyTurn());
        }
        
        public void ClickedQuit()
        {
            throw new System.NotImplementedException();
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

        public IEnumerator VisualiseBattleEvents(Queue<BattleEvent> battleEvents)
        {
            while (battleEvents.Count > 0)
            {
                var nextBattleEvent = battleEvents.Dequeue();
                
                _hudManager.UpdateDisplay(nextBattleEvent.type);
                
                foreach (var visualiser in nextBattleEvent.visualisers)
                    visualiser.LoadNextState();
                
                if (nextBattleEvent.type == BattleEventType.None)
                    continue;
                
                if (nextBattleEvent.type == BattleEventType.PlayerDied)
                    yield break;
                
                if (nextBattleEvent.type == BattleEventType.EnemyDamaged)
                    yield return new WaitForSeconds
                        (ActionExecutionSpeed * nextBattleEvent.damageModificationPackage.ModCount);
                
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