using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace OfferScreen
{
    public class DesignCardRow : MonoBehaviour
    {
        [SerializeField] private bool isDeckRow;

        private ReorderableList _reorderableList;
        
        private bool IsDeckRow => isDeckRow;

        private void Start()
        {
            _reorderableList = GetComponent<ReorderableList>();

            _reorderableList.OnElementGrabbed.AddListener(ElementGrabbed);
            _reorderableList.OnElementDropped.AddListener(ElementDropped);
        }

        public void ElementGrabbed(ReorderableList.ReorderableListEventStruct reorderableListEventStruct)
        {
            var designCard = reorderableListEventStruct.DroppedObject.GetComponent<DesignCard>();

            designCard.HideButtons();
            designCard.PreventLocking();
            OfferScreenEvents.Current.RefreshOffers();
        }

        public void ElementDropped(ReorderableList.ReorderableListEventStruct reorderableListEventStruct)
        {
            var designCard = reorderableListEventStruct.DroppedObject.GetComponent<DesignCard>();
            var designCardRowDroppedInto = reorderableListEventStruct.ToList.GetComponent<DesignCardRow>();

            if (!designCardRowDroppedInto.IsDeckRow)
                designCard.AllowLocking();
            
            // Buy or sell
            if (reorderableListEventStruct.ToList != reorderableListEventStruct.FromList)
            {
                if (designCardRowDroppedInto.isDeckRow)
                {
                    OfferManager.Current.BuyerSeller.Buy(designCard.Design.Cost);
                }
                else
                {
                    OfferManager.Current.BuyerSeller.Sell(designCard.Design.Cost);
                }
            }

            OfferScreenEvents.Current.RefreshOffers();
        }
    }
}
