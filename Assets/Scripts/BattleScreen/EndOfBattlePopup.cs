using Designs.UI;
using Disturbances;
using Pickups.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleScene
{
    public class EndOfBattlePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _headerText;


        [SerializeField] private GameObject _designDisplay;
        [SerializeField] private GameObject _reward;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardTitle;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        [SerializeField] private Button _button;

        public void SetReward(Reward reward)
        {
            _rewardImage.sprite = reward.GetSprite();
            _rewardTitle.text = reward.GetTitle();
            _rewardText.text = reward.GetDescription();
            
            if (reward.RewardType == RewardType.Card ||
                reward.RewardType == RewardType.EliteCard)
            {
                _reward.SetActive(false);
                _designDisplay.SetActive(true);
                _designDisplay.GetComponent<DesignDisplay>().design = (reward as CardReward)?.Design;
            }
        }

        public void SetButtonAction(UnityAction action)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(action);
        }
    }
}
