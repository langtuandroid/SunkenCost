using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOffer : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo ItemInfo { get; set; }
    public Sprite Sprite { get; set; }

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Cost cost;

    private void Start()
    {
        titleText.text = ItemInfo.Title;
        descText.text = ItemInfo.Desc;
        cost.UpdateCost(ItemInfo.Cost);
        image.sprite = Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OfferManager.Current.AcceptOffer(this);
        Destroy(gameObject);
    }
}
