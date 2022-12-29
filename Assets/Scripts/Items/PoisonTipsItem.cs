namespace Items
{
    public class PoisonTipsItem : InGameItem
    {
        public const int PoisonAmount = 1;
    
        protected override void Activate()
        {
            BattleEvents.Current.OnEnemyAttacked += AddPoison;
        }

        private void AddPoison()
        {
            var enemy = BattleEvents.LastEnemyAttacked;
            enemy.stats.AddPoison(PoisonAmount);
        }

        protected override void OnDestroy()
        {
            BattleEvents.Current.OnEnemyAttacked -= AddPoison;
            base.OnDestroy();
        }
    }
}
