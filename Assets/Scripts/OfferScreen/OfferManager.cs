using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfferManager : MonoBehaviour
{
    public static OfferManager Current;
    [SerializeField] private Transform _upperGrid;
    [SerializeField] private Transform _lowerGrid;
    [SerializeField] private GameObject _designCardPrefab;

    public List<Design> Designs { get; private set; }

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
        var upperGridContent = _upperGrid.GetChild(0);
        foreach (var design in Deck.Designs)
        {
            //Debug.Log(design.Title);
            var info = Instantiate(_designCardPrefab, upperGridContent).GetComponentInChildren<DesignInfo>();
            info.design = design;
            
            Designs.Add(design);
        }
        
        var lowerGridContent = _lowerGrid.GetChild(0);
        for (var i = 0; i < 3; i++)
        {
            var info = Instantiate(_designCardPrefab, lowerGridContent).GetComponentInChildren<DesignInfo>();

            var designName = DesignManager.Rarities.ElementAt(Random.Range(0, DesignManager.Rarities.Count)).Key;
            var designType = DesignManager.GetDesignType(designName);
            var design = (Design)Activator.CreateInstance(designType);
            info.design = design;
            Designs.Add(design);
        }
        
        OfferScreenEvents.Current.GridsUpdated();
    }

}
