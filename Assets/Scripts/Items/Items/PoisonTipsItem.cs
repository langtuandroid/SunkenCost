using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Items.Items
{
    public class PoisonTipsItem : EquippedItem
    {
        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                PackageResponseTrigger(BattleEventType.EnemyDamaged, 
                    b => new BattleEventPackage(b.Enemy.stats.AddPoison(Amount)),
                    b => b.source == DamageSource.Etching)
            };
        }
    }
}