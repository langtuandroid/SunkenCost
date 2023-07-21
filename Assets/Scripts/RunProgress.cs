using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Disturbances;
using Items;
using Items.Items;
using Loaders;
using MapScreen;
using OfferScreen;
using Pickups.Rewards;
using UnityEngine;

public class RunProgress : MonoBehaviour
{
    [field: SerializeField] public RunthroughStartingConfig RunthroughStartingConfig { get; private set; }
    
    public static RunProgress Current;

    public PlayerStats PlayerStats { get; private set; }
    public OfferStorage OfferStorage { get; private set; }
    public ItemInventory ItemInventory { get; private set; }
    public int BattleNumber { get; private set; }
    public bool HasGeneratedMapEvents { get; private set; }
    public List<Reward> GeneratedMapDisturbances { get; private set; }

    public List<Scenario> Scenarios { get; private set; }

    private void Awake()
    {
        if (Current)
        {
            Destroy(gameObject);
            return;
        }
        
        Current = this;
    }
    
    public void SelectNextBattle()
    {
        BattleNumber++;
        HasGeneratedMapEvents = false;
        PlayerStats.SetDeckCostToAmount(0);
    }

    public void HaveGeneratedMapEvents(List<Reward> disturbances)
    {
        HasGeneratedMapEvents = true;
        GeneratedMapDisturbances = disturbances;
    }
    
    public void InitialiseRun()
    {
        RewardLoader.LoadRewardAssets();

        PlayerStats = new PlayerStats(RunthroughStartingConfig);
        PlayerStats.InitialiseDeck(RunthroughStartingConfig.GetStartingDesigns());
        
        OfferStorage = new OfferStorage();
        
        ItemInventory = transform.GetChild(0).gameObject.AddComponent<ItemInventory>();
        foreach (var itemInstance in RunthroughStartingConfig.GetStartingItems()) ItemInventory.AddItem(itemInstance);
        
        Scenarios = new List<Scenario>();

        BattleNumber = 1;
    }

    public void AcceptReward(Reward reward)
    {
        //TODO: FIX THIS

        switch (reward.RewardType)
        {
            case RewardType.GoldRush:
                PlayerStats.Gold += reward.Modifier;
                break;
            case RewardType.Heart:
                PlayerStats.Heal(reward.Modifier);
                break;
            case RewardType.None:
                break;
            case RewardType.UpgradeCard:
                break;
            case RewardType.Card:
            case RewardType.EliteCard:
                if (!(reward is CardReward cardDisturbance)) throw new Exception();
                var rewardCard = cardDisturbance.Design;
                rewardCard.SetCost(0);
                PlayerStats.Deck.Add(cardDisturbance.Design);
                break;
            case RewardType.Item:
            case RewardType.EliteItem:
                if (!(reward is ItemReward itemDisturbance)) throw new Exception();
                ItemInventory.AddItem(itemDisturbance.ItemInstance);
                break;
            case RewardType.MaxHealth:
                PlayerStats.MaxHealth += reward.Modifier;
                PlayerStats.Heal(PlayerStats.MaxHealth);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
