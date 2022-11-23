using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesignCardOffer : MonoBehaviour, IPointerClickHandler
{
    public Design design;
    private DesignInfo _designInfo;

    private void Awake()
    {
        _designInfo = transform.GetChild(1).GetComponent<DesignInfo>();
    }

    private void Start()
    {
        _designInfo.design = design;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.current.DesignOfferAccepted();
        Deck.current.AddDesignToDrawPile(design);
    }
}
