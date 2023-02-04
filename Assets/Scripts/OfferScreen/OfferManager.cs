using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfferManager : MonoBehaviour
{
    public static OfferManager Current;

    [SerializeField] private Transform takeGrid;
    [SerializeField] private Transform leaveGrid;
    [SerializeField] private GameObject designCardPrefab;
    [SerializeField] private GameObject itemOfferPrefab;

    [SerializeField] private Transform bootyGrid;

    public List<DesignCard> DesignCards { get; private set; } = new List<DesignCard>();

    public int CardsOnTopRow => takeGrid.childCount;

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
        CreateCardOfferGrids();

        var items = new string[GameProgress.AmountOfItemsToOffer];

        for (var i = 0; i < GameProgress.AmountOfItemsToOffer; i++)
        {
            items[i] = SpawnItem(items);
        }
    }

    public void BackToMap()
    {
        // TODO: Get this somewhere better, make neater
        var deck = takeGrid.GetComponentsInChildren<DesignInfo>().Select(d => d.design)
            .ToList();
        if (deck.Count >= GameProgress.MaxPlanks) deck = deck.GetRange(0, GameProgress.MaxPlanks);
        PlayerInventory.Deck = deck;

        MainManager.Current.LoadMap();
    }
    
    private string SpawnItem(string[] otherItems)
    {
        var itemOfferGameObject = Instantiate(itemOfferPrefab, bootyGrid);
        var itemOffer = itemOfferGameObject.GetComponent<ItemOffer>();

        if (itemOffer is null)
        {
            Debug.Log("Null Item!");
            return null;
        }
        else
        {
            string itemId;
            while (true)
            {
                itemId = ItemManager.GetRandomItem();
                if (otherItems.FirstOrDefault(id => id == itemId) == null)
                    break;
            }
            
            var titleDesc = ItemManager.GetItemTitleAndDescription(itemId);
            itemOffer.ItemId = itemId;
            itemOffer.Sprite = ItemManager.GetItemSprite(itemId);
            itemOffer.Title = titleDesc[0];
            itemOffer.Desc = titleDesc[1];

            return itemId;
        }
    }

    private void CreateCardOfferGrids()
    {
        foreach (var design in PlayerInventory.Deck)
        {
            //Debug.Log(design.Title);
            var newCardObject = Instantiate(designCardPrefab, takeGrid);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();
            info.design = design;
            
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }
        
        for (var i = 0; i < GameProgress.AmountOfCardsToOffer; i++)
        {
            var newCardObject = Instantiate(designCardPrefab, leaveGrid);
            var info = newCardObject.GetComponentInChildren<DesignInfo>();

            var designName = GetDesign();
            var designType = DesignManager.GetDesignType(designName);
            var design = (Design)Activator.CreateInstance(designType);
            info.design = design;
            DesignCards.Add(newCardObject.GetComponent<DesignCard>());
        }

        StartCoroutine(WaitForDesignCardsToInitialise());
    }

    private string GetDesign()
    {
        var currentProgress = GameProgress.BattleNumber;
        var maxProgress = 50.0;
        
        var chancesOnFirstBattle = new[] {0.8, 0.5, 0.05};
        var chancesOnLastPossibleBattle = new[] {0.2, 0.6, 0.2};

        var percentage = currentProgress / maxProgress;

        var currentChances = new double[chancesOnFirstBattle.Length];

        for (var i = 0; i < currentChances.Length; i++)
        {
            currentChances[i] = chancesOnFirstBattle[i] +
                                (chancesOnLastPossibleBattle[i] - chancesOnFirstBattle[i]) * percentage;
            
        }

        var randomNum = (double)Random.value;
        
        // Common
        if (randomNum <= currentChances[0])
        {
            return DesignManager.CommonDesigns[Random.Range(0, DesignManager.CommonDesigns.Count)];
        }

        randomNum -= currentChances[0];
            
        // Uncommon
        if (randomNum <= currentChances[1])
        {
            return DesignManager.UncommonDesigns[Random.Range(0, DesignManager.UncommonDesigns.Count)];
        }

        // Rare
        return DesignManager.RareDesigns[Random.Range(0, DesignManager.RareDesigns.Count)];
    }

    public void Merge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        DesignCards.Remove(cardBeingMerged);
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.GridsUpdated();
    }

    public void AcceptOffer(ItemOffer offerAccepted)
    {
        PlayerInventory.Items.Add(offerAccepted.ItemId);
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.GridsUpdated();
    }
}
