using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Enemies.Enemies
{
    public class MaxolotlEnemy : EliteEnemy, IStartOfTurnAbilityHolder
    {
        private const int HEALING_AMOUNT = 10;

        public override string GetDescription()
        {
            return "Heals all enemies by " + HEALING_AMOUNT + " health each turn";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return EnemySequencer.Current.AllEnemies.Any(e => e.Health < e.MaxHealth);
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            return new BattleEventPackage(
                EnemySequencer.Current.AllEnemies.Where(e => e.Health < e.MaxHealth).Select
                (enemy => enemy.Heal(HEALING_AMOUNT)));
        }
    }
}