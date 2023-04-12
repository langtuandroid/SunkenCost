using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;

namespace Items.Items
{
    public class ExpiredMedicineItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.Type == BattleEventType.EnemyHealed;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            if (!(battleEvent is EnemyHealBattleEvent enemyHealBattleEvent)) throw new UnexpectedBattleEventException(battleEvent);
            
            return DamageHandler.DamageEnemy
                (Amount, enemyHealBattleEvent.enemy, DamageSource.Item, item: this);
        }
    }
}