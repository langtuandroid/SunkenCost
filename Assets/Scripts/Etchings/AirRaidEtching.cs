using Enemies;

namespace Etchings
{
    public class AirRaidEtching : RangedEtching
    {
        protected override void DamageEnemy(Enemy enemy)
        {
            if (enemy.Stick.etching is not DamageEtching)
            {
                var statMod = new StatModifier(design.GetStat(St.StatMultiplier) - 1, StatModType.PercentMult);
                design.Stats[St.Damage].AddModifier(statMod);
                base.DamageEnemy(enemy);
                design.Stats[St.Damage].RemoveModifier(statMod);
            }
            else
            {
                base.DamageEnemy(enemy);
            }
        }
    }
}