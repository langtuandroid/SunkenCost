using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using Items;
using Items.Items;
using Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemLoader : MonoBehaviour
{
    private static ItemLoader _current;
    public static ReadOnlyDictionary<ItemAsset, Type> ItemAssetToTypeDict;
    public static ReadOnlyCollection<ItemAsset> ShopItemAssets;
    public static ReadOnlyCollection<ItemAsset> EliteItemAssets;

    private void Awake()
    {
        if (_current)
        {
            Destroy(gameObject);
            return;
        }
        
        _current = this;
    }

    private void Start()
    {
        // Add all the Item classes to a dictionary with their class name as the key
        var itemClasses = new Dictionary<string, Type>();
        var itemsEnumerable = Extensions.GetAllChildrenOfClassOrNull<EquippedItem>();
        foreach (var type in itemsEnumerable)
        {
            // Remove "Item" from the end of the class name
            var itemName = type.Name.Remove(type.Name.Length - 4, 4);
            itemClasses.Add(itemName, type);
        }
        
        // Add all the ItemAssets to another dictionary - their names must match!
        var itemAssetsEnumerable = Extensions.LoadScriptableObjects<ItemAsset>();
        var itemDictionary = new Dictionary<ItemAsset, Type>();
        foreach (var itemAsset in itemAssetsEnumerable)
        {
            var (key, value) = itemClasses.FirstOrDefault
                (i => i.Key == itemAsset.name);

            if (key == null)
            {
                Debug.Log("ERROR: Couldn't find an item class called " + itemAsset.name);
                return;
            }
            
            itemDictionary.Add(itemAsset, value);
        }
        
        ItemAssetToTypeDict = new ReadOnlyDictionary<ItemAsset, Type>(itemDictionary);

        var allItemAssets = ItemAssetToTypeDict.Select(kvp => kvp.Key).ToArray();
        ShopItemAssets = allItemAssets.GetReadonlyCollection
            (ia => ia.rarity < Rarity.ElitePickup);

        EliteItemAssets  = allItemAssets.GetReadonlyCollection
            (ia => ia.rarity == Rarity.ElitePickup);
    }
}
