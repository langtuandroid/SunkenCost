using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Disturbances;
using Items;
using Items.Items;
using MapScreen;
using OfferScreen;
using UnityEngine;

public class RunProgress : MonoBehaviour
{
    [field: SerializeField] public RunthroughStartingConfig RunthroughStartingConfig { get; private set; }
    
    public static RunProgress Current;

    public PlayerStats PlayerStats { get; private set; }
    public OfferStorage OfferStorage { get; private set; }
    public ItemInventory ItemInventory { get; private set; }
    public int BattleNumber { get; private set; }
    public Disturbance CurrentDisturbance { get; private set; }
    public bool HasGeneratedMapEvents { get; private set; }
    public List<Disturbance> GeneratedMapDisturbances { get; private set; }

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
    
    public void SelectNextBattle(Disturbance disturbance)
    {
        CurrentDisturbance = disturbance;
        BattleNumber++;
        HasGeneratedMapEvents = false;
        PlayerStats.SetDeckCostToAmount(0);
    }

    public void HaveGeneratedDisturbanceEvents(List<Disturbance> disturbances)
    {
        HasGeneratedMapEvents = true;
        GeneratedMapDisturbances = disturbances;
    }
    
    public void InitialiseRun()
    {
        DisturbanceLoader.LoadDisturbanceAssets();

        PlayerStats = new PlayerStats(RunthroughStartingConfig);
        PlayerStats.InitialiseDeck(RunthroughStartingConfig.GetStartingDesigns());
        
        OfferStorage = new OfferStorage();
        
        ItemInventory = transform.GetChild(0).gameObject.AddComponent<ItemInventory>();
        foreach (var itemInstance in RunthroughStartingConfig.GetStartingItems()) ItemInventory.AddItem(itemInstance);
        
        Scenarios = new List<Scenario>();

        BattleNumber = 0;
        CurrentDisturbance = null;
    }
}
