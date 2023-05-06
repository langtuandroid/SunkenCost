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

public class DesignManager : MonoBehaviour
{
    private static DesignManager _current;

    public static ReadOnlyDictionary<DesignAsset, Type> DesignAssetToEtchingTypeDict;

    public static ReadOnlyDictionary<string, DesignAsset> AllDesignAssetsByName;
    
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
        var allDesignAssets = Extensions.LoadScriptableObjects<DesignAsset>();
        
        var allDesignAssetsByName = allDesignAssets.ToDictionary(asset => asset.name);
        AllDesignAssetsByName = new ReadOnlyDictionary<string, DesignAsset>(allDesignAssetsByName);
        
        var designAssetToEtchingTypeDict = new Dictionary<DesignAsset, Type>();

        foreach (var designAsset in allDesignAssets)
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

    public static string GetDescription(Design design)
    {
        var description = "";

        switch (design.Type)
        {
            case DesignType.Melee: case DesignType.LoneWolf: case DesignType.Ambush:
                description = "Attacks enemies landing on this plank for " + design.GetStat(StatType.Damage) + " damage";
                if (design.Type == DesignType.LoneWolf)
                {
                    description += ". " + design.GetStat(StatType.StatFlatModifier) + 
                                  " damage for each other plank you have";
                }

                if (design.Type == DesignType.Ambush)
                {
                    description += ". Increase by " + design.GetStat(StatType.StatFlatModifier) +
                                   " each turn it doesn't attack";
                }

                break;
            case DesignType.Ranged:
            case DesignType.Raid:
            {
                var range = "";
                var minRange = design.GetStat(StatType.MinRange);
                var maxRange = design.GetStat(StatType.MaxRange);
                if (minRange == maxRange)
                {
                    range = design.GetStat(StatType.MinRange) + " plank";
                    if (minRange != 1) range += "s";
                }
                else
                {
                    range = minRange + "-" + maxRange + " planks";
                }

                description = "Attacks enemies landing " + range + " away for " + design.GetStat(StatType.Damage) +
                              " damage";

                if (design.Type == DesignType.Raid)
                    description += ". Deal " + design.GetStat(StatType.StatMultiplier) +
                                   "x damage on non-Attack planks";
                break;
            }
            case DesignType.Area:
            {
                var maxRange = design.GetStat(StatType.MaxRange);
                var distance = maxRange + " plank";
                if (maxRange != 1) distance += "s";
                description = "Attacks all enemies up to " + distance +
                              " away for " + design.GetStat(StatType.Damage) + " damage when an enemy lands within that range";
                break;
            }
            case DesignType.Block: 
                description = "Removes " + design.GetStat(StatType.Block) + " movement from enemies leaving this plank";
                break;
            case DesignType.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + design.GetStat(StatType.StatFlatModifier);
                break;
            case DesignType.Hop:
                description = "Enemies leaving this plank skip the next plank";
                break;
            case DesignType.Reverse:
                description = "Reverses the direction ";
                if (design.Level == 0) description += "of ";
                else description += " and adds 1 movement to ";
                description += " an enemy leaving this plank";
                break;
            case DesignType.Poison:
                description = "Applies " + design.GetStat(StatType.Poison) + " poison to enemies landing on this plank";
                break;
            case DesignType.Focus:
                description = "Enemies on this plank take " + design.GetStat(StatType.StatMultiplier) + "x damage";
                if (design.Level < 2) description += "from Attacks";
                break;
            case DesignType.Rest:
            {
                var playerHealthModifier = design.GetStat(StatType.PlayerHealthModifier);
                description = "At the end of the battle, recover " + playerHealthModifier + " health";
                break;
            }
            case DesignType.Finalise:
                description = "When enemies land on this plank, deal " + design.GetStat(StatType.Damage) +
                              " damage to all enemies on every plank. If any survive, lose " + Math.Abs(design.GetStat(StatType.PlayerHealthModifier)) + " lives";
                break;
            case DesignType.Cauterize:
                var multiplier = design.GetStat(StatType.StatMultiplier);
                var multiText = multiplier > 1 ? multiplier + "x " : "";
                description = "When enemies land on this plank, lower their maximum health by " + multiText +
                              "their poison amount";
                break;
            case DesignType.Research:
                description = "When enemies land on this plank, heal them to full health. Gain " + design.GetStat(StatType.Gold) + 
                    " gold ";
                if (design.Level < 2)
                {
                    description += "if " + design.GetStat(StatType.IntRequirement) + " or more health is healed";
                }
                else
                {
                    description += "per " + design.GetStat(StatType.IntRequirement) + " health healed";
                }
                break;
        }

        description = description.Replace("2x", "double");
        description = description.Replace("3x", "triple");

        if (design.HasStat(StatType.UsesPerTurn))
        {
            var usesPerTurn = design.GetStat(StatType.UsesPerTurn);
            var usesText = usesPerTurn switch
            {
                1 => "once",
                2 => "twice",
                3 => "triple",
                _ => usesPerTurn + "x"
            };
            description += " (" + usesText + " per turn)";
        }
        
        return description;
    }
}
