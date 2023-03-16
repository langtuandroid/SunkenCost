using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using Items;
using Items.Items;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemLoader : MonoBehaviour
{
    private static ItemLoader _current;
    public static readonly Dictionary<ItemAsset, Type> ItemTypes = new Dictionary<ItemAsset, Type>();

    private void Start()
    {
        _current = this;
        
        // Add all the Item classes to a dictionary with their class name as the key
        var itemClasses = new Dictionary<string, Type>();
        var itemsEnumerable = Extensions.GetAllChildrenOfClassOrNull<Item>();
        foreach (var type in itemsEnumerable)
        {
            // Remove "Item" from the end of the class name
            var itemName = type.Name.Remove(type.Name.Length - 4, 4);
            itemClasses.Add(itemName, type);
        }
        
        // Add all the ItemAssets to another dictionary - their names must match!
        var itemAssetsEnumerable = Extensions.GetAllInstancesOrNull<ItemAsset>();
        
        foreach (var itemAsset in itemAssetsEnumerable)
        {
            var (key, value) = itemClasses.FirstOrDefault
                (i => i.Key == itemAsset.name);

            if (key == null)
            {
                Debug.Log("ERROR: Couldn't find an item class called " + itemAsset.name);
                return;
            }
            
            ItemTypes.Add(itemAsset, value);
        }
    }
}
