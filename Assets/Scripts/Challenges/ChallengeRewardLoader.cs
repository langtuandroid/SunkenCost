using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
    public class ChallengeRewardLoader : MonoBehaviour
    {
        private static ChallengeRewardLoader _current;

        private readonly Dictionary<ChallengeRewardType, ChallengeReward> _challengeRewards =
            new Dictionary<ChallengeRewardType, ChallengeReward>();

        private void Start()
        {
            _current = this;

            var challengeRewards = Extensions.GetAllInstancesOrNull<ChallengeReward>();

            foreach (var challengeReward in challengeRewards)
            {
                _challengeRewards.Add(challengeReward.challengeRewardType, challengeReward);
            }
        }

        public static ChallengeReward GetChallengeRewardInfo(ChallengeRewardType challengeRewardType)
        {
            return _current._challengeRewards[challengeRewardType];
        }
    }
}