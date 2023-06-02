using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

namespace Items.Items
{
    public class ExpiredMedicineItem : EquippedItem
    {
        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                AddResponseTrigger(BattleEventType.EnemyHealed, b => DamageEnemy(b.primaryResponderID), 
                    b => !b.Enemy.IsDestroyed)
            };
        }

        private BattleEventPackage DamageEnemy(int enemyResponderID)
        {
            return new BattleEventPackage(DamageHandler.DamageEnemy
                (Amount, enemyResponderID, DamageSource.Item));
        }
    }
}