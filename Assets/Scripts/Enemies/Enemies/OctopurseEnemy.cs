using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Enemies.Enemies
{
    public class OctopurseEnemy : Enemy, IStartOfTurnAbilityHolder
    {
        private bool _hasUsedAbility = false;

        public override string GetDescription()
        {
            return "On it's first turn, it adds 8x your Gold to it's health";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return !_hasUsedAbility;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            _hasUsedAbility = true;
            
            var maxHealthModifier = new StatModifier(8 * Player.Current.Gold, StatModType.Flat);
            return new BattleEventPackage(AddMaxHealthModifier(maxHealthModifier));
        }
    }
}