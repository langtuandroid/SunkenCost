using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Enemies.Enemies
{
    public class OctopurseEnemy : Enemy, IStartOfTurnAbilityHolder
    {
        private bool _hasUsedAbility = false;
        
        protected override void Init()
        {
            Name = "Octopurse";
            Mover.AddMove(2);
            Mover.AddMove(3);
            SetInitialHealth(10);
            Gold = 1;
        }
    
        public override string GetDescription()
        {
            return "On it's first turn, it adds 8x your Gold to it's health";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return _hasUsedAbility;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            _hasUsedAbility = true;
            
            var maxHealthModifier = new StatModifier(8 * Player.Current.Gold, StatModType.Flat);
            
            AddMaxHealthModifier(maxHealthModifier);
            return new BattleEventPackage(CreateEvent(BattleEventType.EnemyEffect));
        }
    }
}