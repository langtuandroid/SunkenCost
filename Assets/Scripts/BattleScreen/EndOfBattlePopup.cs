using System;
using System.Collections.Generic;
using Challenges;
using Disturbances;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleScene
{
    public class EndOfBattlePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI headerText;

        [SerializeField] private GameObject reward;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardTitle;
        [SerializeField] private TextMeshProUGUI rewardText;
        
        [SerializeField] private Transform challengeRewardsGrid;
        [SerializeField] private GameObject challengeRewardPrefab;

        [SerializeField] private Button button;

        public void SetReward(Disturbance disturbance)
        {
            rewardImage.sprite = disturbance.GetSprite();
            rewardTitle.text = disturbance.GetTitle();
            rewardText.text = disturbance.GetDescription();
        }

        public void SwapToChallengeRewards(Challenge[] completedChallenges)
        {
            reward.SetActive(false);

            var newHeaderText = "Challenge";
            if (completedChallenges.Length > 1)
                newHeaderText += "s";
            newHeaderText += " complete!";
            
            headerText.text = newHeaderText;

            foreach (var challenge in completedChallenges)
            {
                var newObj = Instantiate(challengeRewardPrefab, challengeRewardsGrid);
                var challengeButton = newObj.GetComponent<ChallengeButton>();
                challengeButton.Init(challenge);
            }
        }

        public void SetButtonAction(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }
}
