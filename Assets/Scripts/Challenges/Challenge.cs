using System;

namespace Challenges
{
    public abstract class Challenge
    {
        public bool IsActive { get; set; } = true;
        public ChallengeRewardType ChallengeRewardType { get; private set; }
        
        protected int Level { get; private set; }

        public Challenge(ChallengeRewardType challengeRewardType, int level)
        {
            ChallengeRewardType = challengeRewardType;
            Level = level;
        }

        public abstract bool HasAchievedCondition();

        public abstract string GetDescription();
    }
}