using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace OfferScreen
{
    public class CostDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _costText;
        
        private static readonly Color GoodColor = new Color(1f, 0.97f, 0f);
        private static readonly Color BadColor = new Color(0.56f, 0f, 0f);

        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Refresh(int cost, bool canBuy)
        {
            RefreshCost(cost);
            
            var color = canBuy ? GoodColor : BadColor;
            RefreshColor(color);

            if (cost == 0)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        private void RefreshCost(int cost)
        {
            _costText.text = cost.ToString();
        }

        private void RefreshColor(Color color)
        {
            _costText.color = color;
        }
    }
}