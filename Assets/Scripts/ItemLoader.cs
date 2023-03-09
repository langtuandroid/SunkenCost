using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemLoader : MonoBehaviour
{
    private static ItemLoader _current;
    public static readonly Dictionary<string, Type> Items = new Dictionary<string, Type>();

    [SerializeField] private List<string> itemTypes = new List<string>();
    [SerializeField] private List<Sprite> itemSprites = new List<Sprite>();

    private void Start()
    {
        _current = this;
        
        // Get the Etching Offers
        var itemsEnumerable = Extensions.GetAllChildrenOfClassOrNull<InBattleItem>();
        foreach (var type in itemsEnumerable)
        {
            // Remove the 'Items.' from the start and 'Item' from end of file name
            if (type.FullName != null)
            {
                var itemName = type.FullName.Remove(type.FullName.Length - 4, 4).Remove(0, 6);
                Items.Add(itemName, type);
            }
            else
            {
                Debug.Log("What?");
            }
        }
    }

    public static Sprite GetItemSprite(string item)
    {
        var index = _current.itemTypes.IndexOf(item);
        return _current.itemSprites[index];
    }

    public static ItemOffer CreateItemOffer(string itemID)
    {
        var title = "ERROR";
        var desc = "ERROR";
        var rarity = -1;

        switch (itemID)
        {
            case "PoisonTips":
                title = "Poison Tips";
                desc = "Apply " + PoisonTipsItem.PoisonAmount +" poison every time an enemy is attacked";
                rarity = 1;
                break;
            
            case "ExpiredMedicine":
                title = "Expired Medicine";
                desc = "Whenever an enemy heals, it takes " + ExpiredMedicineItem.DamageAmount + " damage";
                rarity = 1;
                break;
            case "ReDress":
                title = "ReDress";
                desc = "Whenever an enemy reaches your boat, deal " + ReDressItem.DamageAmount + " damage to all enemies";
                rarity = 1;
                break;
        }

        return new ItemOffer(itemID, title, desc, rarity);
    }

    public static string GetRandomItem()
    {
        return _current.itemTypes[Random.Range(0, _current.itemTypes.Count)];
    }
}
