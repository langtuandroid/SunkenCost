namespace Enemies.Enemies
{
    public class SwordfistEnemy : Enemy
    {
        public override string GetDescription()
        {
            return "Deals double damage to your boat";
        }

        public override int GetBoatDamage()
        {
            return base.GetBoatDamage() * 2;
        }
    }
}