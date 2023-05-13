namespace Enemies.Enemies
{
    public class BullCarpEnemy : Enemy
    {
        private void Start()
        {
            Gold = 0;
        }

        public override string GetDescription()
        {
            return "Does not drop any gold when killed";
        }
    }
}