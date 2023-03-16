using TMPro;
using UnityEngine;

namespace OfferScreen
{
    public class PlankCount : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField] private TMP_ColorGradient goodColor;
        [SerializeField] private TMP_ColorGradient badColor;

        private void Awake()
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            UpdateText(RunProgress.PlayerStats.Deck.Count);
        }

        public void UpdateText(int numOfPlanksInDeck)
        {
            var maxCards = RunProgress.PlayerStats.MaxPlanks;

            _textMeshProUGUI.text = numOfPlanksInDeck + "/" + maxCards;

            _textMeshProUGUI.colorGradientPreset = numOfPlanksInDeck <= maxCards ? goodColor : badColor;
        }
    }
}
