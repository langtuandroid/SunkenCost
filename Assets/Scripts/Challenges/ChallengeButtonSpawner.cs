using UnityEngine;

namespace Challenges
{
    public class ChallengeButtonSpawner : MonoBehaviour
    {
        [SerializeField] private Transform challengesArea;
        [SerializeField] private GameObject challengeButtonPrefab;

        public void SpawnChallengeButton(Challenge challenge)
        {
            var newObj = Instantiate(challengeButtonPrefab, challengesArea);
            var newChallengeButton = newObj.GetComponent<ChallengeButton>();
            newChallengeButton.Init(challenge);
        }
    }
}