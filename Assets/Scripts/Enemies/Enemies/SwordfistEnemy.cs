namespace Enemies.Enemies
{
    public class SwordfistEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Swordfist";
            Mover.AddMove(2);
            SetInitialHealth(40);
            Gold = 1;
        }

        public override string GetDescription()
        {
            return "Deals double damage to your boat";
        }

        protected override int GetBoatDamage()
        {
            return base.GetBoatDamage() * 2;
        }
    }
}