using System;
using Items;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OfferScreen
{
    public class ItemOfferDisplay : MonoBehaviour, IOffer
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private ItemDisplay itemDisplay;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private LockButton lockButton;
        [SerializeField] private Color lockColor;
        
        public bool isLocked = false;

        public ItemOffer ItemOffer { get; set; }

        private bool CanBuy => ItemOffer.Cost <= OfferManager.Current.BuyerSeller.Gold;
        private void Start()
        {
            var itemInstance = ItemOffer.itemInstance;
            
            itemDisplay.SetItemInstance(itemInstance);
            itemDisplay.SetColor(isLocked ? lockColor : Color.white);
            
            titleText.text = itemInstance.Title;
            
            OfferScreenEvents.Current.OnOffersRefreshed += OffersRefreshed;
        }

        private void OnDestroy()
        {
            OfferScreenEvents.Current.OnOffersRefreshed -= OffersRefreshed;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            lockButton.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            lockButton.Hide();
        }

        public void Click()
        {
            if (OfferManager.Current.BuyerSeller.Gold < ItemOffer.Cost) return;
            
            OfferManager.Current.BuyItem(ItemOffer);
            Destroy(gameObject);
        }

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            itemDisplay.SetColor(isLocked ? lockColor : Color.white);

        }

        private void OffersRefreshed()
        {
            costDisplay.Refresh(ItemOffer.Cost, CanBuy);
        }
    }
}
