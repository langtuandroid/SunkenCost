using System;

namespace Challenges
{
    public abstract class Challenge
    {
        public bool IsActive { get; set; } = true;
        public ChallengeRewardType ChallengeRewardType { get; private set; }

        protected int Progress { get; set; }
        protected int RequiredProgress { get; set; }

        protected Challenge(ChallengeRewardType challengeRewardType)
        {
            ChallengeRewardType = challengeRewardType;
        }

        public bool HasAchievedCondition()
        {
            return Progress >= RequiredProgress;
        }

        public string GetDescriptionWithProgress()
        {
            var desc = GetDescription();

            if (HasAchievedCondition()) return desc;
            
            return desc + " (" + Progress + "/" + RequiredProgress + ")";
        }

        protected abstract string GetDescription();
    }
}