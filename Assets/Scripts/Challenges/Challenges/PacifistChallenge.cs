using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class PacifistChallenge : Challenge, IKillListener, IEndOfBattleListener
    {
        private const int BASE_RUNS_REQUIRED = 3;

        public PacifistChallenge(ChallengeRewardType challengeRewardType, int level) : 
            base(challengeRewardType)
        {
            RequiredProgress = level + BASE_RUNS_REQUIRED;
        }

        protected override string GetDescription()
        {
            return "Kill no enemies in " + RequiredProgress + " consecutive battles";
        }

        public void EnemyKilled()
        {
            Progress = -1;
        }

        public void EndOfBattle()
        {
            Progress++;
        }
    }
}