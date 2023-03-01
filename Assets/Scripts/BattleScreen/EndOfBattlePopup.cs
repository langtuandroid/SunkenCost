using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class EndOfBattlePopup : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;

        public void SetReward(Sprite sprite, string text)
        {
            rewardImage.sprite = sprite;
            rewardText.text = text;
        }
    }
}
