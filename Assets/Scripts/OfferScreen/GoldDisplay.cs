using TMPro;
using UnityEngine;

namespace OfferScreen
{
    public class GoldDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField] private TMP_ColorGradient _goodColor;
        [SerializeField] private TMP_ColorGradient _badColor;

        private void Awake()
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            UpdateText(RunProgress.PlayerStats.Gold);
        }

        public void UpdateText(int goldAmount)
        {
            _textMeshProUGUI.text = "$" + goldAmount.ToString();
            _textMeshProUGUI.colorGradientPreset = goldAmount >= 0 ? _goodColor : _badColor;
        }
    }
}