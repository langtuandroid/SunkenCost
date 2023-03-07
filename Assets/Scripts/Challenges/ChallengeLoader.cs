using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Challenges
{
    public class ChallengeLoader : MonoBehaviour
    {
        private static ChallengeLoader _current;

        private List<Type> _challenges;

        private void Awake()
        {
            _current = this;
            _challenges = Extensions.GetAllChildrenOfClassOrNull<Challenge>().ToList();
        }

        public static Type GetRandomChallengeType(List<Type> previousChallengeTypes)
        {
            var unmatchedElements = _current._challenges.Where(c => !previousChallengeTypes.Contains(c)).ToList();
            return unmatchedElements[Random.Range(0, unmatchedElements.Count())];
        }

        public static Challenge CreateChallenge(Type challengeType)
        {
            var challenge = (Challenge)Activator.CreateInstance(challengeType);
            return challenge;
        }
    }
}