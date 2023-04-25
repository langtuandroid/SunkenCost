using System.Collections.Generic;
using System.Linq;
using Damage;
using Designs;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class BattleEventsManager : MonoBehaviour
    {
        public static BattleEventsManager Current;
        
        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;
        [SerializeField] private BattleEventResponder _player;
        [SerializeField] private EnemyController _enemyController;
        
        private BattleEventResponderGroup[] _responderOrder;

        private List<IBattleEventUpdatedUI> _eventRespondingUI = new List<IBattleEventUpdatedUI>();
        
        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;

            _responderOrder = new []{_enemiesManager, _etchingManager, _itemManager};
        }

        public List<BattleEvent> StartBattle()
        {
            var startBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.StartedBattle, null));

            EnemySpawner.Instance.StartBattle();
            startBattleEvents.AddRange(SpawnEnemies());
            return startBattleEvents;
        }

        public List<BattleEvent> GetNextTurn()
        {
            var turnEvents = new List<BattleEvent>();
            
            // Start of turn processing
            var startTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.StartedEnemyTurn, null));

            // Iterate through the enemies' moves
            var movementBattleEvents = _enemyController.GetMovements();
            
            // End of turn
            var endTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.EndedEnemyTurn, null));

            turnEvents.AddRange(startTurnBattleEvents);
            turnEvents.AddRange(movementBattleEvents);
            turnEvents.AddRange(endTurnBattleEvents);
            turnEvents.AddRange(SpawnEnemies());

            return turnEvents;
        }

        public List<BattleEvent> EndBattle()
        {
            return GetEventAndResponsesList(new BattleEvent(BattleEventType.EndedBattle, null));
        }

        public List<BattleEvent> GetEventAndResponsesList(BattleEvent battleEvent)
        {
            foreach (var ui in _eventRespondingUI.Where(ui => ui.GetIfUpdating(battleEvent)))
            {
                Debug.Log(ui + " saving state!");
                ui.SaveStateResponse(battleEvent.type);
                battleEvent.visualisers.Add(ui);
            }
            
            var responses = new List<BattleEvent> {battleEvent};

            foreach (var responderGroup in _responderOrder)
            {
                var groupResponses = GetResponsesFromGroup(responderGroup, battleEvent);
                responses.AddRange(groupResponses);
            }
            
            if (_player.GetIfRespondingToBattleEvent(battleEvent))
                responses.AddRange(_player.GetResponseToBattleEvent(battleEvent));

            return responses;
        }

        public DamageModificationPackage GetDamageModifiers(BattleEvent battleEvent)
        {
            var modifiers = _responderOrder.Select
                (g => g.GetDamageModifiers(battleEvent)).ToList();
            
            var flatTotal = modifiers.SelectMany(package => package.flatModifications).ToList();
            var multiTotal = modifiers.SelectMany(package => package.multiModifications).ToList();
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }

        public void RegisterUIUpdater(IBattleEventUpdatedUI ui)
        {
            _eventRespondingUI.Add(ui);
        }
        
        public void DeregisterUIUpdater(IBattleEventUpdatedUI ui)
        {
            _eventRespondingUI.Remove(ui);
        }

        private List<BattleEvent> GetResponsesFromGroup(BattleEventResponderGroup group,
            BattleEvent battleEvent)
        {
            var groupResponse = new List<BattleEvent>();

            if (battleEvent.type == BattleEventType.PlayerDied || battleEvent.type == BattleEventType.EndedBattle)
                return groupResponse;
            
            var responders = group.GetEventResponders(battleEvent);

            foreach (var turnActionResponder in responders)
            {
                groupResponse.AddRange(turnActionResponder.GetResponseToBattleEvent(battleEvent));
            }

            return groupResponse;
        }


        private List<BattleEvent> SpawnEnemies()
        {
            var battleEvents = new List<BattleEvent>();
            foreach (var enemySpawnEvent in EnemySpawner.Instance.SpawnNewTurn())
            {
                battleEvents.AddRange(GetEventAndResponsesList(enemySpawnEvent));
            }

            return battleEvents;
        }
    }
}