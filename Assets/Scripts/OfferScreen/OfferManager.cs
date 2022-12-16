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

    public List<DesignCard> DesignCards { get; private set; } = new List<DesignCard>();

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }
        else
        {
            Current = this;
        }
    }

    private void Start()
    {
        
    }

    private void CreateCardOfferGrids()
    {
        var upperGridContent = _upperGrid.GetChild(0);
        foreach (var design in Deck.Designs)
        {
            //Debug.Log(design.Title);
            var newCardObject = Instantiate(_designCardPrefab, upperGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();
            info.design = design;
            
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }
        
        var lowerGridContent = _lowerGrid.GetChild(0);
        for (var i = 0; i < 3; i++)
        {
            var newCardObject = Instantiate(_designCardPrefab, lowerGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();
            
            var zeroRarityDesigns = DesignManager.Rarities.Where(r => r.Value == 0).ToList();
            var designName = zeroRarityDesigns.ElementAt(Random.Range(0, zeroRarityDesigns.Count)).Key;
            var designType = DesignManager.GetDesignType(designName);
            var design = (Design)Activator.CreateInstance(designType);
            info.design = design;
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }

        StartCoroutine(WaitForDesignCardsToInitialise());
    }

    public void Merge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        DesignCards.Remove(cardBeingMerged);
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.GridsUpdated();
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.GridsUpdated();
    }
}
