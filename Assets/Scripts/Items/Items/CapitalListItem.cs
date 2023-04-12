using System;
using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class CapitalListItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.Type == BattleEventType.GainedGold
                   && EnemyController.Current.NumberOfEnemies > 0;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            var randomEnemy = EnemyController.Current.GetRandomEnemy();
            if (!randomEnemy) throw new Exception("Somehow there is no active enemy?");
            return DamageHandler.DamageEnemy(Amount, randomEnemy, DamageSource.Item, item: this);
        }
    }
}