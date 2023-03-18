using Items;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OfferScreen
{
    public class ItemOfferDisplay : MonoBehaviour, IPointerClickHandler, IOffer
    {
        [SerializeField] private TextMeshProUGUI titleText;

        [SerializeField] private ItemDisplay itemDisplay;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private LockButton lockButton;
        [SerializeField] private Color lockColor;
        
        public ItemOffer ItemOffer { get; set; }

        public bool isLocked = false;

        private void Start()
        {
            var itemInstance = ItemOffer.itemInstance;

            var title = itemInstance.Title;
            var desc = itemInstance.Description;

            itemDisplay.SetTitle(title);
            itemDisplay.SetDescription(desc);
            itemDisplay.SetSprite(itemInstance.Sprite);
            itemDisplay.SetBackgroundColor(isLocked ? lockColor : Color.white);
            
            titleText.text = title;
            
            OfferScreenEvents.Current.OnGridsUpdated += OffersRefreshed;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            lockButton.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            lockButton.Hide();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OfferManager.Current.BuyerSeller.Gold < ItemOffer.Cost) return;
            
            OfferManager.Current.BuyItem(ItemOffer);
            Destroy(gameObject);
        }

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            itemDisplay.SetBackgroundColor(isLocked ? lockColor : Color.white);

        }

        private void OffersRefreshed()
        {
            costDisplay.Refresh(ItemOffer.Cost);
        }
    }
}
