using UnityEngine;
using TMPro;

namespace OfferScreen
{
    public class CostDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        
        private static readonly Color GoodColor = new Color(1f, 0.97f, 0f);
        private static readonly Color BadColor = new Color(0.56f, 0f, 0f);

        public void Refresh(int cost)
        {
            RefreshCost(cost);
            
            var color = cost <= OfferManager.Current.BuyerSeller.Gold ? GoodColor : BadColor;
            RefreshColor(color);
        }

        private void RefreshCost(int cost)
        {
            costText.text = cost.ToString();
        }

        private void RefreshColor(Color color)
        {
            costText.color = color;
        }
    }
}