namespace Items
{
    public class ReDressItem : InBattleItem
    {
        public const int DamageAmount = 5;
        
        protected override void Activate()
        {
            BattleEvents.Current.OnEnemyReachedEnd += DamageAllEnemies;
        }

        private void DamageAllEnemies()
        {
            foreach (var enemy in ActiveEnemiesManager.Current.ActiveEnemies)
            {
                DamageHandler.DamageEnemy(DamageAmount, enemy, DamageSource.Item);
            }
        }

        protected override void OnDestroy()
        {
            BattleEvents.Current.OnEnemyReachedEnd -= DamageAllEnemies;
            base.OnDestroy();
        }
    }
}
