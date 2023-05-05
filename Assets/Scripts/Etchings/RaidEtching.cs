using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;

namespace Etchings
{
    public class RaidEtching : RangedEtching
    {
        protected override BattleEvent DamageEnemy(int enemyResponderID)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(enemyResponderID);
            
            BattleEvent battleEvent;
            if (enemy.Plank.Etching is not DamageEtching)
            {
                var statMod = new StatModifier(design.GetStat(StatType.StatMultiplier) - 1, StatModType.PercentMult);
                design.AddStatModifier(StatType.Damage, statMod);
                battleEvent = base.DamageEnemy(enemyResponderID);
                design.RemoveStatModifier(StatType.Damage, statMod);
            }
            else
            {
                battleEvent = base.DamageEnemy(enemyResponderID);
            }

            return battleEvent;
        }
    }
}