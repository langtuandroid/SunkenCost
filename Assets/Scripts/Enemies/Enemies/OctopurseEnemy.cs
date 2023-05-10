using BattleScreen;
using BattleScreen.BattleEvents;

namespace Enemies.Enemies
{
    public class OctopurseEnemy : Enemy, ISpawnAbilityHolder
    {
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
            return "Adds 8x your Gold to it's health when it spawns";
        }

        public BattleEventPackage GetSpawnAbility()
        {
            var maxHealthModifier = new StatModifier(8 * Player.Current.Gold, StatModType.Flat);
            AddMaxHealthModifier(maxHealthModifier);
            return new BattleEventPackage(CreateEvent(BattleEventType.EnemyEffect));
        }
    }
}