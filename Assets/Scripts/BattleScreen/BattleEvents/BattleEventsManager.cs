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
        [SerializeField] private BattleEventResponderGroup _playerManager;
        
        private BattleEventResponderGroup[] _responderGroupOrder;
        
        private readonly BattleEventResponderTracker _battleEventResponderTracker = new BattleEventResponderTracker();

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;

            _responderGroupOrder = new []{_playerManager, _enemiesManager, _etchingManager, _itemManager};
        }
        
        public BattleEventPackage GetNextResponse(BattleEvent battleEvent)
        {
            var index = _battleEventResponderTracker.GetIndex(battleEvent);

            // Check each of the responder groups
            while (index < _responderGroupOrder.Length)
            {
                var responsePackage = _responderGroupOrder[index].GetNextResponse(battleEvent);
                if (!responsePackage.IsEmpty) return responsePackage;

                index++;
                _battleEventResponderTracker.SetIndex(battleEvent, index);
                
                if (index == _responderGroupOrder.Length &&
                    (battleEvent.type == BattleEventType.StartedIndividualEnemyTurn ||
                    battleEvent.type == BattleEventType.EnemyAboutToMove ||
                    battleEvent.type == BattleEventType.EnemyMove ||
                    battleEvent.type == BattleEventType.EnemyStartOfTurnEffect))
                {
                    return new BattleEventPackage(new BattleEvent(BattleEventType.FinishedRespondingToEnemy));
                }
            }

            return BattleEventPackage.Empty;
        }
        
        public DamageModificationPackage GetDamageModifiers(BattleEvent battleEvent)
        {
            var modifiers = _responderGroupOrder.Select
                (g => g.GetDamageModifiers(battleEvent)).ToList();
            
            var flatTotal = modifiers.SelectMany(package => package.flatModifications).ToList();
            var multiTotal = modifiers.SelectMany(package => package.multiModifications).ToList();
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }
        
    }
}