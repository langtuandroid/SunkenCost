using System;
using UnityEngine;
using UnityEngine.UI;

namespace OfferScreen
{
    public class ReRollButton : MonoBehaviour
    {
        [SerializeField] private CostDisplay _costDisplay;
        [SerializeField] private Button _button;
        
        private int Cost => RunProgress.Current.PlayerStats.ReRollCost;
        private bool CanBuy => Cost <= OfferManager.Current.BuyerSeller.Gold;

        private void Awake()
        {
            _button.onClick.AddListener(ReRoll);
        }

        private void Start()
        {
            OfferScreenEvents.Current.OnOffersRefreshed += OffersRefreshed;
        }

        private void ReRoll()
        {
            if (CanBuy) OfferManager.Current.ReRoll(Cost);
        }

        private void OffersRefreshed()
        {
            _costDisplay.Refresh(Cost, CanBuy);
        }
    }
}