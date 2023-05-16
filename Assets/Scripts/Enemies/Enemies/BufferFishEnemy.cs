namespace Enemies.Enemies
{
    public class BufferFishEnemy : Enemy, ILandingStopper
    {
        public override string GetDescription()
        {
            return "Doesn't let anyone else land on his plank";
        }

        public bool GetIfStoppingEnemyLandingOnPlank(int plankNum)
        {
            return true;
        }
    }
}