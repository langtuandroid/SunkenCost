using System;
using OfferScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Challenges
{
    public class ChallengeButton : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image tickImage;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color notSelectedColor;

        private Challenge _challenge;
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Clicked);
        }

        public void Init(Challenge challenge)
        {
            var challengeRewardArchetype = ChallengeArchetypeLoader.GetChallengeArchetype(challenge.ChallengeRewardType);
            iconImage.sprite = challengeRewardArchetype.sprite;
            title.text = challengeRewardArchetype.title;
            
            description.text = challenge.GetDescription();
            _challenge = challenge;
        }

        private void Clicked()
        {
            _challenge.IsActive = !_challenge.IsActive;
            backgroundImage.color = _challenge.IsActive ? selectedColor : notSelectedColor;

            tickImage.enabled = _challenge.IsActive;
        }
    }
}