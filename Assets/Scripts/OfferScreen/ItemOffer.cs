using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OfferScreen
{
    public class ItemOffer : MonoBehaviour, IPointerClickHandler, IOffer
    {
        public ItemInfo ItemInfo { get; set; }
        public Sprite Sprite { get; set; }

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private LockButton lockButton;
        [SerializeField] private Color lockColor;
    
        public bool isLocked = false;

        private void Start()
        {
            titleText.text = ItemInfo.Title;
            descText.text = ItemInfo.Desc;
            costDisplay.Refresh(ItemInfo.Cost);
            image.sprite = Sprite;
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
            OfferManager.Current.AcceptOffer(this);
            Destroy(gameObject);
        }

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            image.color = isLocked ? lockColor : Color.white;

        }
    }
}
