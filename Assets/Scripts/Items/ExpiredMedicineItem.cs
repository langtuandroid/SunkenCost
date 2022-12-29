namespace Items
{
    public class ExpiredMedicineItem : InGameItem
    {
        private const int DamageAmount = 2;

        protected override void Activate()
        {
            BattleEvents.Current.OnEnemyHealed += EnemyHealed;
        }

        private void EnemyHealed()
        {
            BattleEvents.LastEnemyHealed.TakeDamage(DamageAmount);
        }
    }
}
