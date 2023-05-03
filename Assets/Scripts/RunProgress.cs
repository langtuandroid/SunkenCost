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
    private static RunProgress _current;
    private PlayerStats _playerStats;
    private OfferStorage _offerStorage;
    private ItemInventory _itemInventory;

    private int _battleNumber;

    private Disturbance _currentDisturbance;

    private bool _hasGeneratedMapEvents;

    public static PlayerStats PlayerStats => _current._playerStats;
    public static OfferStorage OfferStorage => _current._offerStorage;

    public static ItemInventory ItemInventory => _current._itemInventory;
    
    public static int BattleNumber => _current._battleNumber;

    public static Disturbance CurrentDisturbance => _current._currentDisturbance;

    public static bool HasGeneratedMapEvents => _current._hasGeneratedMapEvents;

    public static List<Disturbance> GeneratedMapDisturbances { get; private set; }

    private void Awake()
    {
        if (_current)
        {
            Destroy(gameObject);
            return;
        }
        
        _current = this;
    }

    public static void Initialise()
    {
        _current.InitialiseRun();
    }

    public static void SelectNextBattle(Disturbance disturbance)
    {
        _current._currentDisturbance = disturbance;
        _current._battleNumber++;
        _current._hasGeneratedMapEvents = false;
    }

    public static void HaveGeneratedDisturbanceEvents(List<Disturbance> disturbances)
    {
        _current._hasGeneratedMapEvents = true;
        GeneratedMapDisturbances = disturbances;
    }
    
    private void InitialiseRun()
    {
        DisturbanceManager.LoadDisturbanceAssets();
        
        _playerStats = new PlayerStats();
        _playerStats.InitialiseDeck();
        _offerStorage = new OfferStorage();
        _itemInventory = transform.GetChild(0).gameObject.AddComponent<ItemInventory>();
        _battleNumber = 0;
        _currentDisturbance = null;
        
        AddItem(typeof(PoisonTipsItem));
        AddItem(typeof(ExpiredMedicineItem));
    }

    // Used to test Items - add to initialise run
    private void AddItem(Type t)
    {
        var item = ItemLoader.ItemAssetToTypeDict.First
            (i => i.Value == t).Key;
        
        _itemInventory.AddItem(new ItemInstance(item, item.modifier));
    }
}
