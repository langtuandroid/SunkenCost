using System;

namespace Challenges
{
    public abstract class Challenge
    {
        public bool IsActive { get; set; } = true;
        public ChallengeRewardType ChallengeRewardType { get; private set; }

        protected int Progress { get; set; }
        protected int RequiredProgress { get; set; }

        public void Initialise(ChallengeRewardType challengeRewardType, int level)
        {
            ChallengeRewardType = challengeRewardType;
            RequiredProgress = GetRequiredProgress(level);
        }

        public bool HasAchievedCondition()
        {
            return Progress >= RequiredProgress;
        }

        public string GetDescriptionWithProgress()
        {
            var desc = GetDescription();

            if (RequiredProgress == 1)
            {
                if (desc.Contains("consecutive battles"))
                    desc = desc.Replace("1 consecutive battles", "a battle");
            }

            if (HasAchievedCondition()) return desc;
            
            return desc + " (" + Progress + "/" + RequiredProgress + ")";
        }

        protected abstract string GetDescription();

        protected abstract int GetRequiredProgress(int level);
    }
}