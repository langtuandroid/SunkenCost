using Designs;
using Enemies;

namespace Etchings
{
    public class RaidEtching : RangedEtching
    {
        protected override void DamageEnemy(Enemy enemy)
        {
            if (enemy.Plank.Etching is not DamageEtching)
            {
                var statMod = new StatModifier(design.GetStat(StatType.StatMultiplier) - 1, StatModType.PercentMult);
                design.Stats[StatType.Damage].AddModifier(statMod);
                base.DamageEnemy(enemy);
                design.Stats[StatType.Damage].RemoveModifier(statMod);
            }
            else
            {
                base.DamageEnemy(enemy);
            }
        }
    }
}