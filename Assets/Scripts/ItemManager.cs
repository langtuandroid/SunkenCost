using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using Items;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager current;
    public Dictionary<string, Type> Items = new Dictionary<string, Type>();

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    private void Start()
    {
        // Get the Etching Offers
        var itemsEnumerable =
            Assembly.GetAssembly(typeof(Item)).GetTypes().Where(t => t.IsSubclassOf(typeof(Item))).Where(ty => !ty.IsAbstract);

        foreach (var type in itemsEnumerable)
        {
            // Remove the 'Items.' from the start and 'Item' from end of file name
            var itemName = type.FullName.Remove(type.FullName.Length - 4, 4).Remove(0, 6);
            Items.Add(itemName, type);
        }
    }

    public void EquipItem(string itemName)
    {
        var gameObject = new GameObject();
        var itemType = Items[itemName];
        gameObject.AddComponent(itemType);
    }
}
