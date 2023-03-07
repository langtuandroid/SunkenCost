using UnityEngine;

namespace Challenges
{
    public class ChallengeButtonSpawner : MonoBehaviour
    {
        [SerializeField] private Transform challengesArea;
        [SerializeField] private GameObject challengeButtonPrefab;

        public void SpawnChallengeButton(Challenge challenge)
        {
            var newTransform = Instantiate(challengeButtonPrefab, challengesArea).transform;
            newTransform.SetSiblingIndex(challengesArea.childCount - 2);
            
            var newChallengeButton = newTransform.GetComponent<ChallengeButton>();
            newChallengeButton.Init(challenge);
        }
    }
}