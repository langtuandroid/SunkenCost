using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class CapitalListItem : EquippedItem
    {
        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.GainedGold
                   && EnemySequencer.Current.NumberOfEnemies > 0;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            var randomEnemy = EnemySequencer.Current.GetRandomEnemy();
            if (!randomEnemy) throw new Exception("Somehow there is no active enemy?");
            return new BattleEventPackage(DamageHandler.DamageEnemy(Amount, randomEnemy, DamageSource.Item, item: this));
        }
    }
}