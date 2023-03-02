using System;
using System.Collections.Generic;
using OfferScreen;
using UnityEngine;

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
            var listOfOfferedChallenges = RunProgress.activeChallenges;
            
            foreach (var challenge in listOfOfferedChallenges)
            {
                _challengeButtonSpawner.SpawnChallengeButton(challenge);
            }

            var amountOfChallengesToGenerate = 
                RunProgress.PlayerInventory.AmountOfChallengesToOffer - listOfOfferedChallenges.Count;

            for (var i = 0; i < amountOfChallengesToGenerate; i++)
            {
                var newChallenge = GenerateChallenge(listOfOfferedChallenges);
                listOfOfferedChallenges.Add(newChallenge);
                _challengeButtonSpawner.SpawnChallengeButton(newChallenge);
            }
        }

        private Challenge GenerateChallenge(List<Challenge> listOfOfferedChallenges)
        {
            return new CleanSheetChallenge(ChallengeRewardType.Plank);
        }
    }
}