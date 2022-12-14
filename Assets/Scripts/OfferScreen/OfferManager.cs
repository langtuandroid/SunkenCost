using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferManager : MonoBehaviour
{
    public static OfferManager Current;
    [SerializeField] private Transform _upperGrid;
    [SerializeField] private GameObject _designCardPrefab;

    private void Awake()
    {
        if (Current)
        {
            Destroy(gameObject);
        }
        else
        {
            Current = this;
        }
    }

    private void Start()
    {
        var grid = _upperGrid.GetChild(0);
        foreach (var design in Deck.Designs)
        {
            //Debug.Log(design.Title);
            var info = Instantiate(_designCardPrefab, grid).GetComponentInChildren<DesignInfo>();
            info.design = design;
        }
    }

}
