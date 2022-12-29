namespace Items
{
    public class PoisonTipsItem : Item
    {
        private int _poisonAmount = 1;
    
        protected override void Activate()
        {
            BattleEvents.Current.OnEnemyAttacked += AddPoison;
        }

        private void AddPoison()
        {
            var enemy = BattleEvents.LastEnemyAttacked;
            enemy.stats.AddPoison(_poisonAmount);
        }

        protected override void OnDestroy()
        {
            BattleEvents.Current.OnEnemyAttacked -= AddPoison;
            base.OnDestroy();
        }
    }
}
