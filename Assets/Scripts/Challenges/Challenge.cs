namespace Challenges
{
    public abstract class Challenge
    {
        public bool IsActive { get; set; } = true;
        public ChallengeRewardType ChallengeRewardType { get; private set; }

        public Challenge(ChallengeRewardType challengeRewardType)
        {
            ChallengeRewardType = challengeRewardType;
        }

        public abstract bool HasAchievedCondition();

        public abstract string GetDescription();
    }
}