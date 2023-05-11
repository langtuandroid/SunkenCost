namespace Enemies.Enemies
{
    public class RatEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Scrat";
            Mover.AddMove(2);
            SetInitialHealth(15);
            Gold = 1;
        }
    
        public override string GetDescription()
        {
            return "Scurries along steadily";
        }
    }
}
