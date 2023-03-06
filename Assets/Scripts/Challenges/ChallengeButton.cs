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
        [SerializeField] private Image crossImage;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color notSelectedColor;

        private Challenge _challenge;

        public void Init(Challenge challenge)
        {
            var challengeRewardArchetype = ChallengeArchetypeLoader.GetChallengeArchetype(challenge.ChallengeRewardType);
            iconImage.sprite = challengeRewardArchetype.sprite;
            title.text = challengeRewardArchetype.title;
            
            description.text = challenge.GetDescriptionWithProgress();
            _challenge = challenge;

            SetAppearance();
        }

        public void Clicked()
        {
            _challenge.IsActive = !_challenge.IsActive;
            SetAppearance();
        }

        private void SetAppearance()
        {
            backgroundImage.color = _challenge.IsActive ? selectedColor : notSelectedColor;
            crossImage.enabled = !_challenge.IsActive;
        }
    }
}