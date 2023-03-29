using EventListeners;

namespace Items.Items
{
    public class ReDressItem : EquippedItem, IEnemyReachedEndListener
    {
        public void EnemyReachedEnd()
        {
            var enemies = ActiveEnemiesManager.Current.ActiveEnemies;
            foreach (var enemy in enemies)
            {
                if (!enemy || enemy.IsDestroyed) continue;
                DamageHandler.DamageEnemy(Amount, enemy, DamageSource.Item);
            }
        }
    }
}