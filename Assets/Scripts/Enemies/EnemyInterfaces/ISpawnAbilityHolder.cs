using BattleScreen.BattleEvents;

namespace Enemies
{
    public interface ISpawnAbilityHolder
    {
        public BattleEventPackage GetSpawnAbility();
    }
}