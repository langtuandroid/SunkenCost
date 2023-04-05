using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class ExpiredMedicineItem : BattleEventResponderItem
    {
        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.EnemyHeal;
        }

        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            yield return StartCoroutine(BattleState.current.DamageHandler.DamageEnemy
                (Amount, OldBattleEvents.LastEnemyHealed, DamageSource.Item));
        }
    }
}