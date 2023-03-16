using System;
using System.Collections.Generic;
using System.Linq;
using Challenges.Challenges;
using OfferScreen;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Challenges
{
    public class ChallengeOfferManager : MonoBehaviour
    {
        private ChallengeButtonSpawner _challengeButtonSpawner;

        private void Awake()
        {
            _challengeButtonSpawner = GetComponent<ChallengeButtonSpawner>();
        }

        private void Start()
        {
            var listOfOfferedChallenges = RunProgress.ActiveChallenges;
            
            foreach (var challenge in listOfOfferedChallenges)
            {
                _challengeButtonSpawner.SpawnChallengeButton(challenge);
            }

            var amountOfChallengesToGenerate = 
                RunProgress.PlayerStats.NumberOfChallengesToOffer - listOfOfferedChallenges.Count;

            for (var i = 0; i < amountOfChallengesToGenerate; i++)
            {
                var newChallenge = GenerateChallenge(listOfOfferedChallenges);
                listOfOfferedChallenges.Add(newChallenge);
                _challengeButtonSpawner.SpawnChallengeButton(newChallenge);
            }
        }

        private static Challenge GenerateChallenge(List<Challenge> listOfOfferedChallenges)
        {
            var types = listOfOfferedChallenges.Select(c => c.GetType()).ToList();
            var challengeType = ChallengeLoader.GetRandomChallengeType(types);
            var challenge = ChallengeLoader.CreateChallenge(challengeType);

            var challengeRewardTypes = listOfOfferedChallenges.Select(c => c.ChallengeRewardType).ToList();

            var allChallengeRewardTypes = Enum.GetValues(typeof(ChallengeRewardType));

            ChallengeRewardType challengeRewardType;

            if (!challengeRewardTypes.Contains(ChallengeRewardType.Plank))
                challengeRewardType = ChallengeRewardType.Plank;
            else while (true)
            {
                challengeRewardType =
                    (ChallengeRewardType)allChallengeRewardTypes
                        .GetValue(Random.Range(0, allChallengeRewardTypes.Length));

                if (!challengeRewardTypes.Contains(challengeRewardType)) break;
            }
            
            
            var level = (int)MathF.Floor(RunProgress.BattleNumber / 5f);
                
            // Plank challenges are harder
            if (challengeRewardType == ChallengeRewardType.Plank) level ++;
                
            challenge.Initialise(challengeRewardType, level);

            
            return challenge;
        }
    }
}