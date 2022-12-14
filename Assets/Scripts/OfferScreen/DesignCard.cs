using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesignCard : MonoBehaviour
{
    [SerializeField] private Transform plusButton;
    private Design _design;

    private void Awake()
    {
        _design = GetComponentInChildren<DesignInfo>().design;
    }

    private void Start()
    {
        OfferScreenEvents.Current.OnGridsUpdated += CardsUpdated;
    }

    private void CardsUpdated()
    {
        var duplicates = OfferManager.Current.Designs.Where(d => d.Title == _design.Title).ToList();
        plusButton.gameObject.SetActive(duplicates.Count > 1);
    }
}
