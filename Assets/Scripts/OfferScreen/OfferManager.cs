using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using OfferScreen;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class OfferManager : MonoBehaviour
{
    [SerializeField] private GoldDisplay goldDisplay;
    [SerializeField] private ItemIconsDisplay itemIconsDisplay;
    
    [SerializeField] private BuyPlankOffer buyPlankOffer;
    [SerializeField] private BuyMoveOffer buyMoveOffer;
    
    public static OfferManager Current;

    private ItemOfferGenerator _itemOfferGenerator;
    private DesignCardOfferGenerator _designCardOfferGenerator;

    public BuyerSeller BuyerSeller { get; private set; }


    public static int CardsOnTopRow => Current._designCardOfferGenerator.CardsInDeckRowCount;

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }

        Current = this;
        
        BuyerSeller = new BuyerSeller(RunProgress.Current.PlayerStats.Gold, goldDisplay);

        _itemOfferGenerator = GetComponent<ItemOfferGenerator>();
        _designCardOfferGenerator = GetComponent<DesignCardOfferGenerator>();
    }

    private void Start()
    {
        UpdateBuyPlankCost();
        OfferScreenEvents.Current.OnOffersRefreshed += UpdateBuyPlankCost;
        
        _itemOfferGenerator.Initialise();
        _designCardOfferGenerator.CreateDesignCards();
        StartCoroutine(WaitForDesignCardsToInitialise());
    }

    public void BackToMap()
    {
        if (BuyerSeller.Gold < 0) return;
        if (CardsOnTopRow > RunProgress.Current.PlayerStats.MaxPlanks) return;

        var deckRow = _designCardOfferGenerator.CardsInDeckRow;
        var offerRow = _designCardOfferGenerator.CardsInLeaveRow;
        var itemOfferDisplays = _itemOfferGenerator.ItemOfferDisplays;
        
        RunProgress.Current.OfferStorage.StoreOffers(deckRow, offerRow, itemOfferDisplays);
        RunProgress.Current.PlayerStats.Gold = BuyerSeller.Gold;
        MainManager.Current.LoadMap();
    }

    public void TryMerge(DesignCard cardBeingMerged, DesignCard cardBeingMergedInto)
    {
        var cost = cardBeingMerged.Design.Cost;
        if (BuyerSeller.Gold < cost) return;
        BuyerSeller.Buy(cost);
        
        Destroy(cardBeingMerged.gameObject);
        cardBeingMergedInto.Design.LevelUp();
        OfferScreenEvents.Current.RefreshOffers();
    }

    public void BuyItem(ItemOffer itemOffer)
    {
        RunProgress.Current.ItemInventory.AddItem(itemOffer.itemInstance);
        BuyerSeller.Buy(itemOffer.Cost);
        OfferScreenEvents.Current.RefreshOffers();
        itemIconsDisplay.AddItemToDisplay(itemOffer.itemInstance);
    }

    public void BuyPlank(int cost)
    {
        BuyerSeller.Buy(cost);
        RunProgress.Current.PlayerStats.BuyPlank();
        OfferScreenEvents.Current.RefreshOffers();
    }

    public void BuyMove(int cost)
    {
        BuyerSeller.Buy(cost);
        RunProgress.Current.PlayerStats.BuyMove();
        OfferScreenEvents.Current.RefreshOffers();
    }

    public void ReRoll(int cost)
    {
        BuyerSeller.Buy(cost);
        _designCardOfferGenerator.ReRoll();
        _itemOfferGenerator.ReRoll();
        RunProgress.Current.PlayerStats.UsedReRoll();
        StartCoroutine(WaitForDesignCardsToInitialise());
    }
    
    private void UpdateBuyPlankCost()
    {
        buyPlankOffer.UpdateCost((RunProgress.Current.PlayerStats.PlanksBought + 1)
            * 25);
    }

    private IEnumerator WaitForDesignCardsToInitialise()
    {
        yield return 0;
        yield return 0;
        OfferScreenEvents.Current.RefreshOffers();
    }
}
