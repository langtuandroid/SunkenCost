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
            var enemy = BattleEventResponseSequencer.Current.GetEnemyByResponderID(enemyResponderID);
            
            BattleEvent battleEvent;
            if (enemy.Plank.Etching is not DamageEtching)
            {
                var statMod = new StatModifier(Design.GetStat(StatType.StatMultiplier), StatModType.Multiply);
                Design.AddStatModifier(StatType.Damage, statMod);
                battleEvent = base.DamageEnemy(enemyResponderID);
                Design.RemoveStatModifier(StatType.Damage, statMod);
            }
            else
            {
                battleEvent = base.DamageEnemy(enemyResponderID);
            }

            return battleEvent;
        }
    }
}