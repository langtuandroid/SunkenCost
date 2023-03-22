using EventListeners;

namespace Items.Items
{
    public class ReDressItem : EquippedItem, IEnemyReachedEndListener
    {
        public void EnemyReachedEnd()
        {
            foreach (var enemy in ActiveEnemiesManager.Current.ActiveEnemies)
            {
                DamageHandler.DamageEnemy(Amount, enemy, DamageSource.Item);
            }
        }
    }
}