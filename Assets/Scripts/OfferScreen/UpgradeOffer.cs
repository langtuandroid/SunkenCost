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

        private void Awake()
        {
            button.onClick.AddListener(Clicked);
        }

        public void UpdateCost(int cost)
        {
            Cost = cost;
            costDisplay.Refresh(cost);
        }

        private void Clicked()
        {
            if (Cost <= OfferManager.Current.BuyerSeller.Gold)
                Buy();
        }

        protected abstract void Buy();
    }
}