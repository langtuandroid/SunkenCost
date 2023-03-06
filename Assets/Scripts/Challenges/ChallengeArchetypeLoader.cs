using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
    public class ChallengeArchetypeLoader : MonoBehaviour
    {
        private static ChallengeArchetypeLoader _current;

        private readonly Dictionary<ChallengeRewardType, ChallengeRewardArchetype> _challengeArchetypes =
            new Dictionary<ChallengeRewardType, ChallengeRewardArchetype>();

        private void Start()
        {
            _current = this;

            var challengeArchetypes = Extensions.GetAllInstancesOrNull<ChallengeRewardArchetype>();

            foreach (var challengeArchetype in challengeArchetypes)
            {
                _challengeArchetypes.Add(challengeArchetype.challengeRewardType, challengeArchetype);
            }
        }

        public static ChallengeRewardArchetype GetChallengeArchetype(ChallengeRewardType challengeRewardType)
        {
            return _current._challengeArchetypes[challengeRewardType];
        }
    }
}