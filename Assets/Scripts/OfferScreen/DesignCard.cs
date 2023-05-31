using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Designs;
using Designs.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OfferScreen
{
    public class DesignCard : MonoBehaviour, IOffer
    {
        public bool isLocked = false;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Color lockedColor;
        [SerializeField] private Image cardBackgroundImage;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private CostDisplay mergeButton;
        [SerializeField] private LockButton lockButton;
        
        private DesignDisplay _designDisplay;
        private DesignCard _mergeableDesignCard;
        private IOffer _offerImplementation;
        private bool _inOfferRow = false;
        private bool _pointerInside = false;

        public Design Design => _designDisplay.design;
        private bool HasMergeableDesignCard => _mergeableDesignCard != null;

        private bool CanBuy => !_inOfferRow || Design.Cost <= OfferManager.Current.BuyerSeller.Gold;
        
        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _designDisplay = GetComponentInChildren<DesignDisplay>();
        }

        private void Start()
        {
            OfferScreenEvents.Current.OnOffersRefreshed += CardsUpdated;
            StartCoroutine(Show());
        }

        private IEnumerator Show()
        {
            yield return 0;
            yield return 0;
            _canvasGroup.alpha = 1f;
            _canvasGroup.enabled = false;
        }
        
        private void OnDestroy()
        {
            OfferScreenEvents.Current.OnOffersRefreshed -= CardsUpdated;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerInside = true;
            
            if (_inOfferRow)
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
            _inOfferRow = false;
            lockButton.Hide();
        }

        public void PreventLocking()
        {
            isLocked = false;
            _inOfferRow = false;
        }

        public void AllowLocking()
        {
            _inOfferRow = true;

            if (_pointerInside)
            {
                lockButton.Show();
            }
        }

        public void Merge()
        {
            OfferManager.Current.TryMerge(_mergeableDesignCard, this);
        }

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            OfferScreenEvents.Current.RefreshOffers();
        }
        
        private void CardsUpdated()
        {
            _mergeableDesignCard = GetAnyMergeableDesignCard();
            mergeButton.gameObject.SetActive(HasMergeableDesignCard);
            
            if (HasMergeableDesignCard)
            {
                mergeButton.Refresh(_mergeableDesignCard.Design.Cost, _mergeableDesignCard.Design.Cost <= OfferManager.Current.BuyerSeller.Gold);
            }

            _designDisplay.UpdateDisplay();

            costDisplay.Refresh(Design.Cost, CanBuy);

            cardBackgroundImage.color = isLocked ? lockedColor : Color.white;
        }
        
        private DesignCard GetAnyMergeableDesignCard()
        {
            if (!Design.Upgradeable || Design.Level >= 2) return null;
            
            return FindObjectsOfType<DesignCard>()
                .Where(d => d != this)
                .Where(d => d.Design.Title == Design.Title)
                .Where(d => !d.HasMergeableDesignCard)
                .FirstOrDefault(d => d.Design.Level < 2);
        }
    }
}
