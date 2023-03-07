using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using OfferScreen;
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

    [SerializeField] private Transform itemGrid;
    [SerializeField] private GoldDisplay goldDisplay;

    public List<DesignCard> AllDesignCards { get; private set; } = new List<DesignCard>();
    public BuyerSeller BuyerSeller { get; private set; }

    public int CardsOnTopRow => takeGrid.childCount;

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }

        Current = this;
        
        BuyerSeller = new BuyerSeller(RunProgress.PlayerProgress.Gold, goldDisplay);
    }

    private void Start()
    {
        CreateDesignCards();

        var itemIds = new List<string>();

        foreach (var itemOffer in RunProgress.OfferStorage.LockedItemOffers)
        {
            CreateItemOfferDisplay(itemOffer, true);
            itemIds.Add(itemOffer.ItemId);
        }

        if (RunProgress.HasGeneratedMapEvents)
        {
            foreach (var itemOffer in RunProgress.OfferStorage.UnlockedItemOffers)
            {
                CreateItemOfferDisplay(itemOffer, false);
                itemIds.Add(itemOffer.ItemId);
            }
        }
        else
        {
            for (var i = RunProgress.OfferStorage.LockedItemOffers.Count;
                i < RunProgress.PlayerProgress.NumberOfItemsToOffer; i++)
            {
                CreateItemOfferDisplay(GenerateNewItemOffer(itemIds), false);
            }
        }
        
    }

    public void BackToMap()
    {
        if (BuyerSeller.Gold < 0) return;
        if (takeGrid.childCount > RunProgress.PlayerProgress.MaxPlanks) return;

        var deckRow = takeGrid.GetComponentsInChildren<DesignCard>();
        var offerRow = leaveGrid.GetComponentsInChildren<DesignCard>();
        var itemOffers = itemGrid.GetComponentsInChildren<ItemOfferDisplay>();
        
        RunProgress.OfferStorage.StoreOffers(deckRow, offerRow, itemOffers);
        MainManager.Current.LoadMap();
    }

    private static ItemOffer GenerateNewItemOffer(IReadOnlyCollection<string> otherItems)
    {
        string itemId;
            
        while (true)
        { 
            itemId = ItemLoader.GetRandomItem(); 
            if (otherItems.FirstOrDefault(id => id == itemId) == null)
                break;
        }

        return ItemLoader.CreateItemOffer(itemId);
    }

    private void CreateItemOfferDisplay(ItemOffer itemOffer, bool isLocked)
    {
        var itemOfferGameObject = Instantiate(itemOfferPrefab, itemGrid);
        var itemOfferDisplay = itemOfferGameObject.GetComponent<ItemOfferDisplay>();
        InitialiseItemOfferDisplay(itemOfferDisplay, itemOffer, isLocked);
    }
    
    private void InitialiseItemOfferDisplay(ItemOfferDisplay itemOfferDisplay, ItemOffer itemOffer, bool isLocked)
    {
        itemOfferDisplay.ItemOffer = itemOffer;
        itemOfferDisplay.Sprite = ItemLoader.GetItemSprite(itemOffer.ItemId);
        itemOfferDisplay.isLocked = isLocked;
    }

    private void CreateDesignCards()
    {
        CreateDeckCards();
        CreateLockedCards();
        
        if (RunProgress.HasGeneratedMapEvents)
        {
            CreateSavedUnlockedCards();
        }
        else
        {
            GenerateNewUnlockedCards();
        }

        StartCoroutine(WaitForDesignCardsToInitialise());
    }
    

    private void CreateDeckCards()
    {
        foreach (var design in RunProgress.PlayerProgress.Deck)
        {
            CreateDesignCardFromDesign(design, takeGrid, lockable: false);
        }
    }

    private void CreateLockedCards()
    {
        foreach (var design in RunProgress.OfferStorage.LockedDesignOffers)
        {
            CreateDesignCardFromDesign(design, leaveGrid, locked: true);
        }
    }

    private void CreateSavedUnlockedCards()
    {
        foreach (var design in RunProgress.OfferStorage.UnlockedDesignOffers)
        {
            CreateDesignCardFromDesign(design, leaveGrid);
        }
    }

    private void GenerateNewUnlockedCards()
    {
        var amountOfLockedCards = RunProgress.OfferStorage.LockedDesignOffers.Count;
        for (var i = amountOfLockedCards; i < RunProgress.PlayerProgress.NumberOfCardsToOffer; i++)
        {
            var designName = GetDesign();
            var designType = DesignManager.GetDesignType(designName);
            var design = (Design)Activator.CreateInstance(designType);

            CreateDesignCardFromDesign(design, leaveGrid);
        }
    }

    private void CreateDesignCardFromDesign(Design design, Transform row, bool locked = false, bool lockable = true)
    {
        var newCardObject = Instantiate(designCardPrefab, row);
        var info = newCardObject.GetComponentInChildren<DesignDisplay>();
        info.design = design;

        var designCard = newCardObject.GetComponent<DesignCard>();
        designCard.isLocked = locked;

        if (lockable)
        {
            designCard.AllowLocking();
        }
        else
        {
            designCard.PreventLocking();
        }
        
        AllDesignCards.Add(designCard);
    }

    private string GetDesign()
    {
        var currentProgress = RunProgress.BattleNumber;
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
    
    private void CreateLockedItems()
    {
        foreach (var itemOffer in RunProgress.OfferStorage.LockedItemOffers)
        {
            
        }
    }

    public void TryMerge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        var cost = cardBeingMergedInto.Design.Cost;
        if (BuyerSeller.Gold < cost) return;
        BuyerSeller.Buy(cost);

        AllDesignCards.Remove(cardBeingMerged);
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.RefreshOffers();
    }

    public void BuyItem(ItemOffer itemOffer)
    {
        RunProgress.PlayerProgress.Items.Add(itemOffer.ItemId);
        BuyerSeller.Buy(itemOffer.Cost);
        OfferScreenEvents.Current.RefreshOffers();
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.RefreshOffers();
    }
}
