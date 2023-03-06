using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class WipeoutChallenge : Challenge, IEndOfBattleListener
    {
        private const int BASE_WIPEOUTS_REQUIRED = 1;

        public WipeoutChallenge(ChallengeRewardType challengeRewardType, int level) : 
            base(challengeRewardType)
        {
            RequiredProgress = level + BASE_WIPEOUTS_REQUIRED;
        }

        protected override string GetDescription()
        {
            return "Kill every enemy in " + RequiredProgress + " consecutive battles";
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