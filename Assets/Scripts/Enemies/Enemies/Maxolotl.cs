namespace Enemies.Enemies
{
    public class Maxolotl : Enemy
    {
        private const int HEALING_AMOUNT = 5;
        
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

        protected override void StartOfTurnAbility()
        {
            foreach (var enemy in ActiveEnemiesManager.Current.ActiveEnemies)
            {
                enemy.Heal(HEALING_AMOUNT);
            }
            
            base.ExecuteStartOfTurnAbility();
        }

        protected override bool HasStartOfTurnAbility()
        {
            return true;
        }
    }
}