using Disturbances;
using Pickups.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapScreen
{

    public class DisturbanceEvent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Image innerWash;
        [SerializeField] private Sprite blueInnerWash;
        [SerializeField] private Sprite redInnerWash;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;

        public Reward Reward { get; private set; }

        public void Init(Reward reward)
        {
            Reward = reward;
        }
        
        private void Start()
        {
            backgroundImage.enabled = Reward.RewardType == RewardType.Card 
                                      || Reward.RewardType == RewardType.EliteCard;
            foregroundImage.sprite = Reward.GetSprite();
            titleText.text = Reward.GetTitle();
            descriptionText.text = Reward.GetDescription();

            innerWash.sprite = Reward.IsElite ? redInnerWash : blueInnerWash;
        }
        
        public void NextBattle()
        {
            MainManager.Current.LoadNextBattle();
        }

        public void UpdateDescription(string desc)
        {
            descriptionText.text = desc;
        }
    }
}
