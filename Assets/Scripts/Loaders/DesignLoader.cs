using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Designs;
using Etchings;
using Items;
using UnityEngine;

public enum DesignType
{
    Melee,
    Ranged,
    Area,
    Block,
    Boost,
    Hop,
    Reverse,
    Poison,
    Focus,
    Rest,
    LoneWolf,
    Finalise,
    Ambush,
    Cauterize,
    Raid,
    Research
}

public class DesignLoader : MonoBehaviour
{
    private static DesignLoader _current;

    public static ReadOnlyDictionary<DesignAsset, Type> DesignAssetToEtchingTypeDict;

    public static readonly List<DesignAsset> AllDesignAssets = new List<DesignAsset>();
    
    public static readonly List<DesignAsset> CommonDesigns = new List<DesignAsset>();
    public static readonly List<DesignAsset> UncommonDesigns = new List<DesignAsset>();
    public static readonly List<DesignAsset> RareDesigns = new List<DesignAsset>();
    public static readonly List<DesignAsset> ElitePickupDesigns = new List<DesignAsset>();

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
        // Add all the etchings to a dictionary where their class name is the key
        var etchingsEnumerable = 
            Assembly.GetAssembly(typeof(Etching)).GetTypes().Where(t => t.IsSubclassOf(typeof(Etching))).Where(ty => !ty.IsAbstract);

        var etchingNamesAndTypes = new Dictionary<string, Type>();        
        foreach (var type in etchingsEnumerable)
        {
            // // Remove the 'Etchings.' and 'Etching' from beginning and end of file name
            if (type.FullName == null) continue;
            var etchingName = type.FullName.Remove(type.FullName.Length - 7, 7).Remove(0, 9);
            etchingNamesAndTypes.Add(etchingName, type);
        }
        
        // Now match all of the designs to their corresponding etching using their DesignType - note the names
        // of the etchings must match the corresponding DesignType!
        AllDesignAssets.AddRange(Extensions.LoadScriptableObjects<DesignAsset>());

        var designAssetToEtchingTypeDict = new Dictionary<DesignAsset, Type>();

        foreach (var designAsset in AllDesignAssets)
        {
            switch (designAsset.rarity)
            {
                case Rarity.Common:
                    CommonDesigns.Add(designAsset);
                    break;
                case Rarity.Uncommon:
                    UncommonDesigns.Add(designAsset);
                    break;
                case Rarity.Rare:
                    RareDesigns.Add(designAsset);
                    break;
                case Rarity.ElitePickup:
                    ElitePickupDesigns.Add(designAsset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var designTypeName = Enum.GetName(typeof(DesignType), designAsset.designType);
            
            var matchingEtchingType = etchingNamesAndTypes.FirstOrDefault
                (i => i.Key == designTypeName).Value;

            if (matchingEtchingType == null)
            {
                Debug.Log("ERROR: Couldn't find an Etching class called " + designTypeName);
                return;
            }
            
            designAssetToEtchingTypeDict.Add(designAsset, matchingEtchingType);
        }
        
        DesignAssetToEtchingTypeDict = new ReadOnlyDictionary<DesignAsset, Type>(designAssetToEtchingTypeDict);
    }
}
