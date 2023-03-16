using EventListeners;

namespace Items.Items
{
    public class ConfusedItem : Item, IHasPickupAction, IStartOfBattleListener
    {
        private StatModifier _extraPlank;

        public void OnPickup()
        {
            _extraPlank = new StatModifier(Amount, StatModType.Flat);
            RunProgress.PlayerStats.maxPlanks.AddModifier(_extraPlank);
        }

        public void StartOfBattle()
        {
            StickManager.current.RandomisePlanks();
        }

        public void OnDestroy()
        {
            RunProgress.PlayerStats.maxPlanks.RemoveModifier(_extraPlank);
        }
    }
}