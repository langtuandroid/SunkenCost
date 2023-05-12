using Designs.UI;
using Disturbances;
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

        public void SetReward(Disturbance disturbance)
        {
            _rewardImage.sprite = disturbance.GetSprite();
            _rewardTitle.text = disturbance.GetTitle();
            _rewardText.text = disturbance.GetDescription();
            
            if (disturbance.DisturbanceType == DisturbanceType.Card ||
                disturbance.DisturbanceType == DisturbanceType.EliteCard)
            {
                _reward.SetActive(false);
                _designDisplay.SetActive(true);
                _designDisplay.GetComponent<DesignDisplay>().design = (disturbance as CardDisturbance)?.Design;
            }
        }

        public void SetButtonAction(UnityAction action)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(action);
        }
    }
}
