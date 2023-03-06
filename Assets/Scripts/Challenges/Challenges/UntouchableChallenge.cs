using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class UntouchableChallenge : Challenge, IPlayerLostLifeListener, IEndOfBattleListener
    {
        private const int BASE_RUNS_REQUIRED = 1;
        
        public UntouchableChallenge(ChallengeRewardType challengeRewardType, int level) : base(challengeRewardType)
        {
            RequiredProgress = level + BASE_RUNS_REQUIRED;
        }

        public void PlayerLostLife()
        {
            Progress = -1;
        }

        public void EndOfBattle()
        {
            Progress++;
        }
        
        protected override string GetDescription()
        {
            return "Finish " + RequiredProgress + " consecutive battles without losing a life";
        }
    }
}