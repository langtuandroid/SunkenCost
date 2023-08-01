using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

namespace Items.Items
{
    public class CapitalListItem : EquippedItem
    {
        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                PackageResponseTrigger(BattleEventType.AlteredGold, b => DamageRandomEnemy(), 
                    b => EnemySequencer.Current.NumberOfEnemies > 0)
            };
        }

        private BattleEventPackage DamageRandomEnemy()
        {
            var randomEnemy = EnemySequencer.Current.GetRandomEnemy();
            if (!randomEnemy) throw new Exception("Somehow there is no active enemy?");
            return new BattleEventPackage(DamageHandler.DamageEnemy(Amount, randomEnemy.ResponderID, DamageSource.Item));
        }
    }
}