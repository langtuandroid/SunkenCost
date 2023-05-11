using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Enemies.Enemies
{
    public class MaxolotlEnemy : EliteEnemy, IStartOfTurnAbilityHolder
    {
        private const int HEALING_AMOUNT = 10;
        
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Maxolotl";
            Mover.AddMove(2);
            Mover.AddMove(3);
            SetInitialHealth(40);
            Gold = 5;
        }

        public override string GetDescription()
        {
            return "Heals all enemies by " + HEALING_AMOUNT + " health each turn";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return true;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            return new BattleEventPackage(EnemySequencer.Current.AllEnemies.Select
                (enemy => enemy.Heal(HEALING_AMOUNT)));
        }
    }
}