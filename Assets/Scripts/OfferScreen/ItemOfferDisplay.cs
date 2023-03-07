using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OfferScreen
{
    public class ItemOfferDisplay : MonoBehaviour, IPointerClickHandler, IOffer
    {
        public ItemOffer ItemOffer { get; set; }
        public Sprite Sprite { get; set; }

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private LockButton lockButton;
        [SerializeField] private Color lockColor;
        [SerializeField] private TooltipTrigger tooltipTrigger;
    
        public bool isLocked = false;

        private void Start()
        {
            titleText.text = tooltipTrigger.header = ItemOffer.Title;
            descText.text = tooltipTrigger.content = ItemOffer.Desc;
            image.sprite = Sprite;

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
            backgroundImage.color = isLocked ? lockColor : Color.white;

        }

        private void OffersRefreshed()
        {
            costDisplay.Refresh(ItemOffer.Cost);
        }
    }
}
