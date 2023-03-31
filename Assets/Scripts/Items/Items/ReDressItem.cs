using EventListeners;

namespace Items.Items
{
    public class ReDressItem : EquippedItem, IEnemyReachedEndListener
    {
        public void EnemyReachedEnd()
        {
            var enemies = ActiveEnemiesManager.Current.ActiveEnemies;
            for (var i = enemies.Count - 1 ; i > 0; i--)
            {
                if (!enemies[i] || enemies[i].IsDestroyed) continue;
                DamageHandler.DamageEnemy(Amount, enemies[i], DamageSource.Item);
            }
        }
    }
}