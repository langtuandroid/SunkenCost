using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class WipeoutChallenge : Challenge, IEndOfBattleListener
    {
        private const int BASE_WIPEOUTS_REQUIRED = 1;

        protected override string GetDescription()
        {
            return "Kill every enemy in " + RequiredProgress + " consecutive battles";
        }
        
        protected override int GetRequiredProgress(int level)
        {
            return level + BASE_WIPEOUTS_REQUIRED;
        }

        public void EndOfBattle()
        {
            if (ActiveEnemiesManager.NumberOfActiveEnemies > 0)
            {
                Progress = 0;
                return;
            }

            Progress++;
        }
    }
}