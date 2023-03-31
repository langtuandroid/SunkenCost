namespace Enemies.Enemies
{
    public class BullCarpEnemy : Enemy
    {
        protected override void Init()
        {
            Name = "Bull Carp";
            Mover.AddMove(2);
            SetInitialHealth(20);
            Gold = 0;
        }

        public override string GetDescription()
        {
            return "Does not drop any gold when killed";
        }
    }
}