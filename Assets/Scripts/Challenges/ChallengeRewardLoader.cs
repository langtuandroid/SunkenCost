using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
    public class ChallengeRewardLoader : MonoBehaviour
    {
        private static ChallengeRewardLoader _current;

        private readonly Dictionary<ChallengeRewardType, ChallengeReward> _challengeRewards =
            new Dictionary<ChallengeRewardType, ChallengeReward>();
        
        private void Awake()
        {
            if (_current)
            {
                Destroy(gameObject);
                return;
            }
        
            _current = this;
        }

        private void Start()
        {
            var challengeRewards = Extensions.LoadScriptableObjects<ChallengeReward>();

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