using System;
using System.Collections.Generic;
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
                RunProgress.PlayerProgress.NumberOfChallengesToOffer - listOfOfferedChallenges.Count;

            for (var i = 0; i < amountOfChallengesToGenerate; i++)
            {
                var newChallenge = GenerateChallenge(listOfOfferedChallenges);
                listOfOfferedChallenges.Add(newChallenge);
                _challengeButtonSpawner.SpawnChallengeButton(newChallenge);
            }
        }

        private Challenge GenerateChallenge(List<Challenge> listOfOfferedChallenges)
        {
            return new PacifistChallenge(ChallengeRewardType.Move, 0);
        }
    }
}