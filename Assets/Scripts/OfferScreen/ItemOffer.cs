using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOffer : MonoBehaviour, IPointerClickHandler
{
    public string ItemName { get; set; } = "PoisonTips";

    public void OnPointerClick(PointerEventData eventData)
    {
        OfferManager.Current.AcceptOffer(ItemName);
    }
}
