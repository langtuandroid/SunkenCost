using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

namespace Items.Items
{
    public class ReDressItem : EquippedItem
    {
        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                AddResponseTrigger(BattleEventType.EnemyAttackedBoat, 
                    b => new BattleEventPackage(DamageHandler.DamageEnemy(Amount, b.primaryResponderID, DamageSource.Item)),
                    b => !b.Enemy.IsDestroyed)
            };
        }
    }
}