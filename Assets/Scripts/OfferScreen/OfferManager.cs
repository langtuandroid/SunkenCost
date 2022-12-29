using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfferManager : MonoBehaviour
{
    public static OfferManager Current;

    [SerializeField] private Transform cardGrid;
    [SerializeField] private GameObject designCardPrefab;
    [SerializeField] private GameObject itemOfferPrefab;

    [SerializeField] private Transform bootyGrid;

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
        /*
        if (GameProgress.BattleNumber % 3 != 0)
        {
            bootyGrid.gameObject.SetActive(false);
            CreateCardOfferGrids();
        }
        */
        
        SpawnItem();
    }

    private void SpawnItem()
    {
        var itemOffer = Instantiate(itemOfferPrefab, bootyGrid.GetChild(0)).GetComponent<ItemOffer>();

        if (itemOffer is null)
        {
            Debug.Log("Null Item!");
        }
        else
        {
            var itemId = ItemManager.GetRandomItem();
            var titleDesc = ItemManager.GetItemTitleAndDescription(itemId);
            itemOffer.ItemId = itemId;
            itemOffer.Sprite = ItemManager.GetItemSprite(itemId);
            itemOffer.Title = titleDesc[0];
            itemOffer.Desc = titleDesc[1];
        }
    }

    private void CreateCardOfferGrids()
    {
        cardGrid.gameObject.SetActive(true);
        
        var upperGridContent = cardGrid.GetChild(0).GetChild(0);
        foreach (var design in PlayerInventory.Deck)
        {
            //Debug.Log(design.Title);
            var newCardObject = Instantiate(designCardPrefab, upperGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();
            info.design = design;
            
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }
        
        var lowerGridContent = cardGrid.GetChild(1).GetChild(0);
        for (var i = 0; i < 3; i++)
        {
            var newCardObject = Instantiate(designCardPrefab, lowerGridContent);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();

            var zeroRarityDesigns = DesignManager.Rarities[0].Concat(DesignManager.Rarities[1]).Concat(DesignManager.Rarities[2]).ToArray();
            var designName = zeroRarityDesigns[Random.Range(0, zeroRarityDesigns.Length)];
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

    public void AcceptOffer(string offerAccepted)
    {
        bootyGrid.gameObject.SetActive(false);

        if (offerAccepted == "Plank")
        {
            GameProgress.MaxPlanks++;
        }
        else
        {
            PlayerInventory.Items.Add(offerAccepted);
        }
        
        CreateCardOfferGrids();
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.GridsUpdated();
    }
}
