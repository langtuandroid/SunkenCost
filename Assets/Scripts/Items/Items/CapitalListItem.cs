using System;
using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class CapitalListItem : BattleEventResponderItem
    {
        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.GainGold && BattleState.current.EnemyController.NumberOfEnemies > 0;
        }

        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            var randomEnemy = BattleState.current.EnemyController.GetRandomEnemy();
            if (!randomEnemy) throw new Exception("Somehow there is no active enemy?");
            yield return StartCoroutine(BattleState.current.DamageHandler.DamageEnemy(Amount, randomEnemy, DamageSource.Item, item: this));
        }
    }
}