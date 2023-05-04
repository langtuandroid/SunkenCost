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
        design.Stats.TryGetValue(StatType.MinRange, out var minRange);
        design.Stats.TryGetValue(StatType.MaxRange, out var maxRange);
        design.Stats.TryGetValue(StatType.Damage, out var damage);
        design.Stats.TryGetValue(StatType.Boost, out var boost);
        design.Stats.TryGetValue(StatType.Block, out var block);
        design.Stats.TryGetValue(StatType.Poison, out var poison);
        design.Stats.TryGetValue(StatType.HealPlayer, out var healPlayer);
        design.Stats.TryGetValue(StatType.StatMultiplier, out var statMultiplier);
        design.Stats.TryGetValue(StatType.Gold, out var gold);
        design.Stats.TryGetValue(StatType.IntRequirement, out var intRequirement);
        design.Stats.TryGetValue(StatType.ModifyPlayerHealth, out var modifyPlayerHealth);

        switch (design.Type)
        {
            case DesignType.Melee: case DesignType.LoneWolf: case DesignType.Ambush:  
                description = "Attacks enemies landing on this plank for " + damage?.Value + " damage";
                if (design.Type == DesignType.LoneWolf)
                {
                    description += ". " + design.Stats[StatType.DamageFlatModifier].Value + 
                                  " damage for each other plank you have";
                }

                if (design.Type == DesignType.Ambush)
                {
                    description += ". Increase by " + design.GetStat(StatType.DamageFlatModifier) +
                                   " each turn it doesn't attack";
                }

                break;
            case DesignType.Ranged: case DesignType.Raid:
                var range = "";
                if (minRange?.Value == maxRange?.Value)
                {
                    range = minRange?.Value.ToString() + " plank";
                    if (minRange?.Value != 1) range += "s";
                }
                else
                {
                    range = minRange?.Value + "-" + maxRange?.Value + " planks";
                }
                
                description = "Attacks enemies landing " + range + " away for " + damage?.Value + " damage";

                if (design.Type == DesignType.Raid)
                    description += ". Deal " + statMultiplier?.Value + "x damage on non-Attack planks";
                break;
            case DesignType.Area:
                var distance = maxRange?.Value + " plank";
                if (maxRange?.Value != 1) distance += "s";
                description = "Attacks all enemies up to " + distance +
                              " away for " + damage?.Value + " damage when an enemy lands within that range";
                break;
            case DesignType.Block: 
                description = "Removes " + block?.Value + " movement from enemies leaving this plank";
                break;
            case DesignType.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + boost?.Value;
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
                description = "Applies " + poison?.Value + " poison to enemies landing on this plank";
                break;
            case DesignType.Focus:
                description = "Enemies on this plank take " + statMultiplier?.Value + " damage";
                if (design.Level < 2) description += "from Attacks";
                break;
            case DesignType.Rest:
                description = "At the end of the battle, recover " + healPlayer?.Value;
                if (healPlayer?.Value > 1) description += " lives";
                else description += " life";
                break;
            case DesignType.Finalise:
                description = "When enemies land on this plank, deal " + damage?.Value +
                              " damage to all enemies on every plank. If any survive, lose " + Math.Abs(modifyPlayerHealth.Value) + " lives";
                break;
            case DesignType.Cauterize:
                var multiplier = statMultiplier?.Value;
                var multiText = multiplier > 1 ? multiplier + "x " : "";
                description = "When enemies land on this plank, lower their maximum health by " + multiText +
                              "their poison amount";
                break;
            case DesignType.Research:
                description = "When enemies land on this plank, heal them to full health. Gain " + gold?.Value + 
                    " gold ";
                if (design.Level < 2)
                {
                    description += "if " + intRequirement?.Value + " or more health is healed";
                }
                else
                {
                    description += "per " + intRequirement?.Value + " health healed";
                }
                break;
        }

        description = description.Replace("2x", "double");
        description = description.Replace("3x", "triple");

        if (design.Stats.TryGetValue(StatType.UsesPerTurn, out var usesPerTurn))
        {
            var usesText = usesPerTurn?.Value switch
            {
                1 => "once",
                2 => "twice",
                3 => "triple",
                _ => usesPerTurn?.Value + "x"
            };
            description += " (" + usesText + " per turn)";
        }
        
        return description;
    }
}
