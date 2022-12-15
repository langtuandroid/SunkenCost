using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesignCard : MonoBehaviour
{
    [SerializeField] private Transform plusButton;

    private DesignInfo _designInfo;
    public Design Design => _designInfo.design;

    private List<DesignCard> _duplicates;

    private void Awake()
    {
        _designInfo = GetComponentInChildren<DesignInfo>();
    }

    private void Start()
    {
        OfferScreenEvents.Current.OnGridsUpdated += CardsUpdated;
    }

    private void CardsUpdated()
    {
        if (!Design.Upgradeable) return;
        
        _duplicates = OfferManager.Current.DesignCards.Where(d => d.Design.Title == Design.Title).Where(d => d != this).ToList();
        plusButton.gameObject.SetActive(_duplicates.Count > 0);
        _designInfo.Refresh();
    }

    public void Merge()
    {
        OfferManager.Current.Merge(this, _duplicates[0]);
    }

    private void OnDestroy()
    {
        OfferScreenEvents.Current.OnGridsUpdated -= CardsUpdated;
    }
}
