using EventListeners;

namespace Items.Items
{
    public class ExpiredMedicineItem : Item, IEnemyHealedListener
    {
        public void EnemyHealed()
        {
            DamageHandler.DamageEnemy(Amount, BattleEvents.LastEnemyHealed, DamageSource.Item);
        }
    }
}