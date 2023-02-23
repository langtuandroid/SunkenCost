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
        
        BuyerSeller = new BuyerSeller(RunProgress.PlayerInventory.Gold, goldDisplay);
    }

    private void Start()
    {
        CreateDesignCards();

        var items = new string[RunProgress.PlayerInventory.AmountOfItemsToOffer];

        for (var i = 0; i < RunProgress.PlayerInventory.AmountOfItemsToOffer; i++)
        {
            items[i] = SpawnItem(items);
        }
    }

    public void BackToMap()
    {
        if (BuyerSeller.Gold < 0) return;
        if (takeGrid.childCount > RunProgress.PlayerInventory.MaxPlanks) return;

        var deckRow = takeGrid.GetComponentsInChildren<DesignCard>();
        var offerRow = leaveGrid.GetComponentsInChildren<DesignCard>();
        var itemOffers = itemGrid.GetComponentsInChildren<ItemOffer>();
        
        RunProgress.offerStorage.StoreOffers(deckRow, offerRow, itemOffers);
        MainManager.Current.LoadMap();
    }
    
    private string SpawnItem(string[] otherItems)
    {
        var itemOfferGameObject = Instantiate(itemOfferPrefab, itemGrid);
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
                itemId = ItemLoader.GetRandomItem();
                if (otherItems.FirstOrDefault(id => id == itemId) == null)
                    break;
            }

            itemOffer.ItemInfo = ItemLoader.GetItemInfo(itemId);
            itemOffer.Sprite = ItemLoader.GetItemSprite(itemId);

            return itemId;
        }
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
        foreach (var design in RunProgress.PlayerInventory.Deck)
        {
            CreateDesignCardFromDesign(design, takeGrid, lockable: false);
        }
    }

    private void CreateLockedCards()
    {
        foreach (var design in RunProgress.offerStorage.LockedDesignOffers)
        {
            CreateDesignCardFromDesign(design, leaveGrid, locked: true);
        }
    }

    private void CreateSavedUnlockedCards()
    {
        foreach (var design in RunProgress.offerStorage.UnlockedDesignOffers)
        {
            CreateDesignCardFromDesign(design, leaveGrid);
        }
    }

    private void GenerateNewUnlockedCards()
    {
        var amountOfLockedCards = RunProgress.offerStorage.LockedDesignOffers.Count;
        for (var i = amountOfLockedCards; i < RunProgress.PlayerInventory.AmountOfCardsToOffer; i++)
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
        var info = newCardObject.GetComponentInChildren<DesignInfo>();
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

    public void Merge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        AllDesignCards.Remove(cardBeingMerged);
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.GridsUpdated();
    }

    public void AcceptOffer(ItemOffer offerAccepted)
    {
        RunProgress.PlayerInventory.Items.Add(offerAccepted.ItemInfo.ItemId);
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        OfferScreenEvents.Current.GridsUpdated();
    }
}
