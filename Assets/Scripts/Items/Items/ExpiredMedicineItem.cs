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

            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            return !enemy.IsDestroyed;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(DamageHandler.DamageEnemy
                (Amount, battleEvent.affectedResponderID, DamageSource.Item));
        }
    }
}