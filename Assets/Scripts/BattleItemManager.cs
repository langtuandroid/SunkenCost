using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemManager : MonoBehaviour
{
    public static BattleItemManager Current;

    private readonly List<Item> _activeItems = new List<Item>();
    public static List<Item> ActiveItems => Current._activeItems;
    
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemGrid;

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }

        Current = this;
    }

    private void Start()
    {
        foreach (var item in PlayerInventory.Items)
        {
            EquipItem(item);
        }
    }
    
    public static void EquipItem(string itemName)
    {
        var newItem = Instantiate(Current.itemPrefab, Current.itemGrid);
        var itemType = ItemManager.Items[itemName];
        newItem.AddComponent(itemType);
        newItem.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = ItemManager.GetItemSprite(itemName);
        
        ActiveItems.Add(newItem.GetComponent<Item>());
    }

}
