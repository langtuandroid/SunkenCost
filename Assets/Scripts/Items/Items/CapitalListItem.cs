using EventListeners;

namespace Items.Items
{
    public class CapitalListItem : EquippedItem, IGainedGoldListener
    {
        public void GainedGold()
        {
            var randomEnemy = ActiveEnemiesManager.GetRandomActiveEnemy();
            if (!randomEnemy) return;
            DamageHandler.DamageEnemy(Amount, randomEnemy, DamageSource.Item);
        }
    }
}