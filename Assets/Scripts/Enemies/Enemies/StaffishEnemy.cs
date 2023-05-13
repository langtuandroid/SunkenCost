using BattleScreen;
using BattleScreen.BattleEvents;
using Stats;

namespace Enemies.Enemies
{
    public class StaffishEnemy : Enemy, IStartOfTurnAbilityHolder
    {
        public override string GetDescription()
        {
            return "Deals double damage to your boat";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return true;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            var maxHealthModifier = new StatModifier(stats.GetModifier(EnemyModifierType.Health), StatModType.Flat);
            return new BattleEventPackage(AddMaxHealthModifier(maxHealthModifier));
        }
    }
}