using System;
using UnityEngine;
using UnityEngine.UI;

namespace OfferScreen
{
    public abstract class UpgradeOffer : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private CostDisplay costDisplay;

        protected int Cost { get; private set; }

        private bool CanBuy => Cost <= OfferManager.Current.BuyerSeller.Gold;

        private void Awake()
        {
            button.onClick.AddListener(Clicked);
        }

        public void UpdateCost(int cost)
        {
            Cost = cost;
            costDisplay.Refresh(cost, CanBuy);
        }

        private void Clicked()
        {
            if (CanBuy)
                Buy();
        }

        protected abstract void Buy();
    }
}