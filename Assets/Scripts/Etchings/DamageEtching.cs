using Designs;
using Enemies;

namespace Etchings
{
    public abstract class DamageEtching : CharMovementActivatedEtching
    {
        protected int Damage => GetStatValue(StatType.Damage);
        protected int MinRange => GetStatValue(StatType.MinRange);
        protected int MaxRange => GetStatValue(StatType.MaxRange);

        protected virtual void DamageEnemy(Enemy enemy)
        {
            DamageHandler.DamageEnemy(Damage, enemy, DamageSource.Plank);
        }

        public void ModifyStat(StatType stat, int amount)
        {
            if (!design.Stats.ContainsKey(stat)) return;
        
            design.Stats[stat].AddModifier(new StatModifier(amount, StatModType.Flat));
            designDisplay.Refresh();
        }
    }
}
