using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents.EventTypes;
using Damage;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class BattleEventsManager : MonoBehaviour
    {
        public static BattleEventsManager Current;

        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;
        [SerializeField] private EnemyController _enemyController;
        
        private BattleEventResponderGroup[] _responderOrder;
        
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
            return GetEventAndResponsesList(new BattleEvent(BattleEventType.StartBattle));
        }

        public List<BattleEvent> GetNextTurn()
        {
            var turnEvents = new List<BattleEvent>();
            
            // Start of turn processing
            var startTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.StartedEnemyTurn));

            // Iterate through the enemies' moves
            var movementBattleEvents = _enemyController.GetMovements();
            
            // End of turn
            var endTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.EndedEnemyTurn));

            turnEvents.AddRange(startTurnBattleEvents);
            turnEvents.AddRange(movementBattleEvents);
            turnEvents.AddRange(endTurnBattleEvents);

            return turnEvents;
        }

        public List<BattleEvent> GetEventAndResponsesList(BattleEvent battleEvent)
        {
            var responses = new List<BattleEvent> {battleEvent};

            foreach (var responderGroup in _responderOrder)
            {
                var groupResponses = GetResponsesFromGroup(responderGroup, battleEvent);
                responses.AddRange(groupResponses);
            }

            return responses;
        }

        public DamageModificationPackage GetDamageModifiers(EnemyDamageBattleEvent enemyDamageBattleEvent)
        {
            var modifiers = _responderOrder.Select
                (g => g.GetDamageModifiers(enemyDamageBattleEvent)).ToList();
            
            var flatTotal = modifiers.SelectMany(package => package.flatModifications).ToList();
            var multiTotal = modifiers.SelectMany(package => package.multiModifications).ToList();
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }

        private List<BattleEvent> GetResponsesFromGroup(BattleEventResponderGroup group,
            BattleEvent battleEvent)
        {
            var groupResponse = new List<BattleEvent>();
            
            var responders = group.GetEventResponders(battleEvent);
            
            foreach (var turnActionResponder in responders)
            {
                groupResponse.AddRange(turnActionResponder.GetResponseToBattleEvent(battleEvent));
            }

            return groupResponse;
        }

        #region Events

        #endregion
    }
}