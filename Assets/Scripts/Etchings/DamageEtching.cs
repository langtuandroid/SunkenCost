using BattleScreen;
using Designs;
using Enemies;

namespace Etchings
{
    public abstract class DamageEtching : LandedOnPlankActivatedEtching
    {
        protected int Damage => GetStatValue(StatType.Damage);
        protected int MinRange => GetStatValue(StatType.MinRange);
        protected int MaxRange => GetStatValue(StatType.MaxRange);

        protected virtual BattleEvent DamageEnemy(Enemy enemy)
        {
            return DamageHandler.DamageEnemy(Damage, enemy, DamageSource.Etching, this);
        }

        public void ModifyStat(StatType stat, int amount)
        {
            if (!design.Stats.ContainsKey(stat)) return;
        
            design.Stats[stat].AddModifier(new StatModifier(amount, StatModType.Flat));
        }
    }
}