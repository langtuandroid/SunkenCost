using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OfferScreen
{
    public class DesignCard : MonoBehaviour, IOffer
    {
        public bool isLocked = false;

        [SerializeField] private Color lockedColor;
        [SerializeField] private Image cardBackgroundImage;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private Transform mergeButton;
        [SerializeField] private LockButton lockButton;
        
        private DesignInfo _designInfo;
        private List<DesignCard> _duplicates = new List<DesignCard>();
        private IOffer _offerImplementation;
        private bool _canBeLocked = false;
        private bool _pointerInside = false;

        public Design Design => _designInfo.design;
        
        private void Awake()
        {
            _designInfo = GetComponentInChildren<DesignInfo>();
        }

        private void Start()
        {
            OfferScreenEvents.Current.OnGridsUpdated += CardsUpdated;
        }
        
        private void OnDestroy()
        {
            OfferScreenEvents.Current.OnGridsUpdated -= CardsUpdated;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerInside = true;
            
            if (_canBeLocked)
            {
                lockButton.Show();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerInside = false;
            lockButton.Hide();
        }

        public void HideButtons()
        {
            mergeButton.gameObject.SetActive(false);
            _canBeLocked = false;
            lockButton.Hide();
        }

        public void PreventLocking()
        {
            isLocked = false;
            _canBeLocked = false;
        }

        public void AllowLocking()
        {
            _canBeLocked = true;

            if (_pointerInside)
            {
                lockButton.Show();
            }
        }

        public void Merge()
        {
            OfferManager.Current.Merge(this, _duplicates[0]);
        }

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            OfferScreenEvents.Current.GridsUpdated();
        }
        
        private void CardsUpdated()
        {
            // Can't upgrade if it's already level 2
            if (!Design.Upgradeable || Design.Level >= 2)
            {
                mergeButton.gameObject.SetActive(false);
                _duplicates.Clear();
            }
            else
            {
                // Check if there's more copies of this card to merge with
                _duplicates = OfferManager.Current.AllDesignCards.Where(d => d.Design.Title == Design.Title)
                    .Where(d => d != this)
                    .Where(d => d.Design.Level < 2).ToList();
                mergeButton.gameObject.SetActive(_duplicates.Count > 0);
            }
        
            _designInfo.Refresh();

            costDisplay.Refresh(Design.GetStat(St.Rarity));

            cardBackgroundImage.color = isLocked ? lockedColor : Color.white;
        }
    }
}
