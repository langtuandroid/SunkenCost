namespace Items
{
    public class ExpiredMedicineItem : InBattleItem
    {
        public const int DamageAmount = 2;

        protected override void Activate()
        {
            BattleEvents.Current.OnEnemyHealed += EnemyHealed;
        }

        private void EnemyHealed()
        {
            DamageHandler.DamageEnemy(DamageAmount, BattleEvents.LastEnemyHealed, DamageSource.Item);
        }
    }
}
