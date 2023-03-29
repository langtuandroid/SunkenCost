namespace Enemies.Enemies
{
    public class CribEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Crib";
            Mover.AddMove(2);
            Mover.AddMove(-1);
            SetInitialHealth(35);
            Gold = 1;
        }
    
        public override string GetDescription()
        {
            return "Two steps forward, one step back";
        }
    }
}