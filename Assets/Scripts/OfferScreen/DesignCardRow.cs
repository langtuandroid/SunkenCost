using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace OfferScreen
{
    public class DesignCardRow : MonoBehaviour
    {
        [SerializeField] private bool isDeckRow;
        private bool IsDeckRow => isDeckRow;

        private void Start()
        {
            var reorderableList = GetComponent<ReorderableList>();

            reorderableList.OnElementGrabbed.AddListener(ElementGrabbed);
            reorderableList.OnElementDropped.AddListener(ElementDropped);
        }

        public void ElementGrabbed(ReorderableList.ReorderableListEventStruct reorderableListEventStruct)
        {
            var designCard = reorderableListEventStruct.DroppedObject.GetComponent<DesignCard>();
            designCard.HideButtons();
            designCard.PreventLocking();
            OfferScreenEvents.Current.GridsUpdated();
        }

        public void ElementDropped(ReorderableList.ReorderableListEventStruct reorderableListEventStruct)
        {
            var designCard = reorderableListEventStruct.DroppedObject.GetComponent<DesignCard>();
            var rowDroppedInto = reorderableListEventStruct.ToList.GetComponent<DesignCardRow>();

            if (!rowDroppedInto.IsDeckRow)
                designCard.AllowLocking();

            OfferScreenEvents.Current.GridsUpdated();
        }
    }
}
