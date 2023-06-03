using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Designs;
using Designs.UI;
using ReorderableContent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OfferScreen
{
    [RequireComponent(typeof(ReorderableElement))]
    public class DesignCard : MonoBehaviour, IOffer, IMergeableReorderableEventListener
    {
        public bool isLocked = false;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Color lockedColor;
        [SerializeField] private Image cardBackgroundImage;
        [SerializeField] private CostDisplay costDisplay;
        [SerializeField] private CostDisplay mergeButton;
        [SerializeField] private LockButton lockButton;
        
        private DesignDisplay _designDisplay;
        private IOffer _offerImplementation;
        private bool _inOfferRow = false;
        private bool _pointerInside = false;

        private ReorderableGrid _listHoveringOverOrIn;

        public Design Design => _designDisplay.design;

        private bool CanBuy => !_inOfferRow || Design.Cost <= OfferManager.Current.BuyerSeller.Gold;
        
        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _designDisplay = GetComponentInChildren<DesignDisplay>();
        }

        private void Start()
        {
            _listHoveringOverOrIn = transform.parent.GetComponentInParent<ReorderableGrid>();
            GetComponent<ReorderableElement>().Init(_listHoveringOverOrIn, this);
            
            OfferScreenEvents.Current.OnOffersRefreshed += CardsUpdated;
            StartCoroutine(Init());
        }

        private IEnumerator Init()
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

        public void ClickedLockButton()
        {
            isLocked = !isLocked;
            OfferScreenEvents.Current.RefreshOffers();
        }

        public void Grabbed()
        {
            HideButtons();
            PreventLocking();
            _designDisplay.DisallowHover();
            OfferScreenEvents.Current.RefreshOffers();
        }

        public void HoveringOverList(ReorderableGrid listHoveringOver)
        {
            if (_listHoveringOverOrIn == listHoveringOver) return;
            
            _listHoveringOverOrIn = listHoveringOver;
            
            var rowDroppedInto = _listHoveringOverOrIn.GetComponent<DesignCardRow>();

            if (rowDroppedInto.IsDeckRow)
            {
                OfferManager.Current.BuyerSeller.Buy(Design.Cost);
            }
            else
            {
                AllowLocking();
                OfferManager.Current.BuyerSeller.Sell(Design.Cost);
            }
            
            OfferScreenEvents.Current.RefreshOffers();
        }

        public void Released()
        {
            _canvasGroup.interactable = true;
            _designDisplay.AllowHover();
            OfferScreenEvents.Current.RefreshOffers();
        }

        public Func<ReorderableElement, bool> GetIfCanMerge()
        {
            return element => CanMerge(element.GetComponent<DesignCard>().Design);
        }

        public void StartMerge()
        {
            cardBackgroundImage.color = lockedColor;
            OfferScreenEvents.Current.RefreshOffers();
        }

        public void FinaliseMerge(ReorderableElement element)
        {
            OfferManager.Current.MergeDesignCards( this, element.GetComponent<DesignCard>());
        }

        public void CancelMerge()
        {
            cardBackgroundImage.color = Color.white;
        }

        private bool CanMerge(Design design)
        {
            return design.Title == Design.Title && Design.Level < 2 && design.Level < 2;
        }

        private void HideButtons()
        {
            mergeButton.gameObject.SetActive(false);
            _inOfferRow = false;
            lockButton.Hide();
        }
        
        private void CardsUpdated()
        {
            _designDisplay.UpdateDisplay();
            costDisplay.Refresh(Design.Cost, CanBuy);
            cardBackgroundImage.color = isLocked ? lockedColor : Color.white;
        }
    }
}
