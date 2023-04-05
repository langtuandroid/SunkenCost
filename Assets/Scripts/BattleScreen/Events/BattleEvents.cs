using System.Collections;
using System.Collections.Generic;
using Damage;
using UnityEngine;

namespace BattleScreen
{
    public class BattleActions : MonoBehaviour
    {
        public const float ActionExecutionSpeed = 0.3f;

        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;

        [SerializeField] private EnemyController _enemyController;

        private IEnumerator StartBattle()
        {
            var startBattleAction = new BattleEvent(BattleEventType.StartBattle);
            yield return StartCoroutine(ExecuteTurnActionResponses(startBattleAction));
        }

        public IEnumerator ExecuteTurn()
        {
            // Start of turn processing
            var startTurnAction = new BattleEvent(BattleEventType.StartTurn);
            yield return StartCoroutine(ExecuteTurnActionResponses(startTurnAction));
            
            // Iterate through the enemies' moves
            yield return StartCoroutine(_enemyController.ExecuteEnemyTurn());
            
            // End of turn
            var endTurnAction = new BattleEvent(BattleEventType.EndTurn);
            yield return StartCoroutine(ExecuteTurnActionResponses(endTurnAction));
        }

        public IEnumerator ExecuteTurnActionResponses(BattleEvent previousBattleEvent)
        {
            yield return StartCoroutine(ExecuteTurnActionGroupResponses(_etchingManager, previousBattleEvent));
            yield return StartCoroutine(ExecuteTurnActionGroupResponses(_itemManager, previousBattleEvent));
            yield return StartCoroutine(ExecuteTurnActionGroupResponses(_enemiesManager, previousBattleEvent));
        }

        public DamageModificationPackage GetDamageModifiers(DamageBattleEvent damageBattleEvent)
        {
            var enemyModifiers = _enemiesManager.GetDamageModifiers(damageBattleEvent);
            var etchingModifiers = _etchingManager.GetDamageModifiers(damageBattleEvent);
            var itemModifiers = _itemManager.GetDamageModifiers(damageBattleEvent);

            var flatTotal = new List<DamageModification>();
            flatTotal.AddRange(enemyModifiers.flatModifications);
            flatTotal.AddRange(etchingModifiers.flatModifications);
            flatTotal.AddRange(itemModifiers.flatModifications);
            
            var multiTotal = new List<DamageModification>();
            multiTotal.AddRange(enemyModifiers.multiModifications);
            multiTotal.AddRange(etchingModifiers.multiModifications);
            multiTotal.AddRange(itemModifiers.multiModifications);
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }

        private IEnumerator ExecuteTurnActionGroupResponses(BattleEventResponderGroup group,
            BattleEvent previousBattleEvent)
        {
            var responders = group.GetTurnActionsToExecute(previousBattleEvent);
            if (responders.Length <= 0) yield break;
            
            foreach (var turnActionResponder in responders)
            {
                yield return StartCoroutine(turnActionResponder.ExecuteResponseToAction(previousBattleEvent));
            }
        }
    }
}