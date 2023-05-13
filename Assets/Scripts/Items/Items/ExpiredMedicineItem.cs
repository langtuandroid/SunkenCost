using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Items.Items
{
    public class ExpiredMedicineItem : EquippedItem
    {
        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type != BattleEventType.EnemyHealed) return false;
            return !battleEvent.Enemy.IsDestroyed;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(DamageHandler.DamageEnemy
                (Amount, battleEvent.primaryResponderID, DamageSource.Item));
        }
    }
}