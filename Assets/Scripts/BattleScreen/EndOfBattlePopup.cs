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
        
        [SerializeField] private Button button;

        public void SetReward(Disturbance disturbance)
        {
            rewardImage.sprite = disturbance.GetSprite();
            rewardTitle.text = disturbance.GetTitle();
            rewardText.text = disturbance.GetDescription();
        }

        public void SetButtonAction(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }
}
