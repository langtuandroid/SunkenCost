using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class ExpiredMedicineItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyHealed;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            return DamageHandler.DamageEnemy
                (Amount, battleEvent.enemyAffectee, DamageSource.Item, item: this);
        }
    }
}