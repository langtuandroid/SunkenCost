namespace Items.Items
{
    public abstract class ExtraPlankItem : EquippedItem, IHasPickupAction
    {
        private StatModifier _extraPlank;

        public virtual void OnPickup()
        {
            _extraPlank = new StatModifier(Amount, StatModType.Flat);
            RunProgress.PlayerStats.maxPlanks.AddModifier(_extraPlank);
        }
        
        public virtual void OnDestroy()
        {
            RunProgress.PlayerStats.maxPlanks.RemoveModifier(_extraPlank);
        }
    }
}