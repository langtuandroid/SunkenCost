using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOffer : MonoBehaviour, IPointerClickHandler
{
    public string ItemId;
    public string Title { get; set; } = "ERROR";
    public string Desc { get; set; } = "ERROR";
    
    public Sprite Sprite { get; set; }

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    private void Start()
    {
        titleText.text = Title;
        descText.text = Desc;
        image.sprite = Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OfferManager.Current.AcceptOffer(ItemId);
    }
}
