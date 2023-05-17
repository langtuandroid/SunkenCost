using Items;
using TMPro;
using UnityEngine;

namespace OfferScreen
{
    public class GoldDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField] private BattleActivationShaderDisplay _shaderDisplay;
        [SerializeField] private TMP_ColorGradient _goodColor;
        [SerializeField] private TMP_ColorGradient _badColor;

        private int _currentGold = -1;

        private void Awake()
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            UpdateText(RunProgress.Current.PlayerStats.Gold);
        }

        public void UpdateText(int goldAmount)
        {
            _textMeshProUGUI.text = "<font=\"GrapeSoda SDF\"><cspace=-0.2em>$ </cspace></font>"
                                    + goldAmount;
            _textMeshProUGUI.colorGradientPreset = goldAmount >= 0 ? _goodColor : _badColor;

            if (_currentGold != goldAmount && _currentGold != -1)
            {
                _shaderDisplay.Activate();
            }
            
            _currentGold = goldAmount;
        }
    }
}