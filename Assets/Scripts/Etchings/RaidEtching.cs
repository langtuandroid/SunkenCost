using BattleScreen;
using Designs;
using Enemies;

namespace Etchings
{
    public class RaidEtching : RangedEtching
    {
        protected override BattleEvent DamageEnemy(Enemy enemy)
        {
            BattleEvent battleEvent;
            if (enemy.Plank.Etching is not DamageEtching)
            {
                var statMod = new StatModifier(design.GetStat(StatType.StatMultiplier) - 1, StatModType.PercentMult);
                design.Stats[StatType.Damage].AddModifier(statMod);
                battleEvent = base.DamageEnemy(enemy);
                design.Stats[StatType.Damage].RemoveModifier(statMod);
            }
            else
            {
                battleEvent = base.DamageEnemy(enemy);
            }

            return battleEvent;
        }
    }
}