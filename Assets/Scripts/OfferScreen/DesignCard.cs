using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesignCard : MonoBehaviour
{
    [SerializeField] private Transform plusButton;
    [SerializeField] private Cost cost;

    private DesignInfo _designInfo;
    public Design Design => _designInfo.design;

    private List<DesignCard> _duplicates = new List<DesignCard>();

    public bool CameFromDeck = false;

    private void Awake()
    {
        _designInfo = GetComponentInChildren<DesignInfo>();
    }

    private void Start()
    {
        OfferScreenEvents.Current.OnGridsUpdated += CardsUpdated;
        
        if (CameFromDeck) cost.gameObject.SetActive(false);
    }

    private void CardsUpdated()
    {
        if (!Design.Upgradeable || Design.Level >= 2)
        {
            plusButton.gameObject.SetActive(false);
            _duplicates.Clear();
        }
        else
        {
            _duplicates = OfferManager.Current.DesignCards.Where(d => d.Design.Title == Design.Title)
                .Where(d => d != this)
                .Where(d => d.Design.Level < 2).ToList();
            plusButton.gameObject.SetActive(_duplicates.Count > 0);
        }
        
        _designInfo.Refresh();
        cost.UpdateCost(Design.GetStat(St.Rarity));
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
