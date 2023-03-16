using EventListeners;

namespace Items.Items
{
    public class PoisonTipsItem : Item, IEnemyAttackedListener
    {
        public void EnemyAttacked()
        {
            var enemy = BattleEvents.LastEnemyAttacked;
            enemy.stats.AddPoison(Amount);
        }
    }
}