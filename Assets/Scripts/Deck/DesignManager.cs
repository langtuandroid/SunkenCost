using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deck;
using Etchings;
using UnityEngine;

public enum DesignCategory
{
    Melee,
    Ranged,
    Area,
    Block,
    Boost,
    Hop,
    Reverse,
    Poison,
    StrikeZone,
    Infirmary,
    LoneWolf,
    GrandFinalist,
    Ambush,
    Cauterize
}

public class DesignManager : MonoBehaviour
{
    private static DesignManager _current;
    private static readonly Dictionary<string, Type> DesignTypes = new Dictionary<string, Type>();
    
    private static readonly Dictionary<string, Type> EtchingTypes = new Dictionary<string, Type>();
    
    [SerializeField] private List<string> etchingTypes = new List<string>();
    [SerializeField] private List<Sprite> etchingSprites = new List<Sprite>();

    public static readonly List<string> CommonDesigns = new List<string>();
    public static readonly List<string> UncommonDesigns = new List<string>();
    public static readonly List<string> RareDesigns = new List<string>();
    
    private void Start()
    {
        _current = this;

        // If reloading
        if (EtchingTypes.Count > 0) return;
        
        // Get the Designs
        var designs = Extensions.GetAllChildrenOfClassOrNull<Design>();

        foreach (var type in designs)
        {
            var designName = type.FullName;
            DesignTypes.Add(designName ?? "ERROR", type);

            if (type.IsSubclassOf(typeof(CommonDesign)))
            {
                CommonDesigns.Add(designName);
            }
            else if (type.IsSubclassOf(typeof(UncommonDesign)))
            {
                UncommonDesigns.Add(designName);
            }
            else if (type.IsSubclassOf(typeof(RareDesign)))
            {
                RareDesigns.Add(designName);
            }
            else
            {
                Debug.Log("ERROR: NOT OF ANY RARITY??");
                Debug.Log(designName);
            }
        }

        // Get the Etchings
        var etchingsEnumerable = 
            Assembly.GetAssembly(typeof(ActiveEtching)).GetTypes().Where(t => t.IsSubclassOf(typeof(ActiveEtching))).Where(ty => !ty.IsAbstract);

        foreach (var type in etchingsEnumerable)
        {
            // // Remove the 'Etchings.' and 'Etching' from beginning and end of file name
            var etchingName = type.FullName.Remove(type.FullName.Length - 7, 7).Remove(0, 9);
            EtchingTypes.Add(etchingName, type);
        }
    }
    
    public static Type GetEtchingType(DesignCategory designCategory)
    {
        //Debug.Log(etchingCategory.ToString());
        return EtchingTypes[designCategory.ToString()];
    }

    public static Type GetDesignType(string designName)
    {
        return DesignTypes[designName];
    }

    public static Sprite GetEtchingSprite(DesignCategory designCategory)
    {
        var index = _current.etchingTypes.IndexOf(designCategory.ToString());
        return _current.etchingSprites[index];
    }

    public static string GetDescription(Design design)
    {
        var description = "";
        design.Stats.TryGetValue(St.MinRange, out var minRange);
        design.Stats.TryGetValue(St.MaxRange, out var maxRange);
        design.Stats.TryGetValue(St.Damage, out var damage);
        design.Stats.TryGetValue(St.Boost, out var boost);
        design.Stats.TryGetValue(St.Block, out var block);
        design.Stats.TryGetValue(St.Poison, out var poison);
        design.Stats.TryGetValue(St.HealPlayer, out var healPlayer);
        design.Stats.TryGetValue(St.MaxHealthMultiplier, out var maxHealthMultiplier);

        switch (design.Category)
        {
            case DesignCategory.Melee: case DesignCategory.LoneWolf: case DesignCategory.Ambush:  
                description = "Attacks enemies landing on this plank for " + damage?.Value + " damage";
                if (design.Category == DesignCategory.LoneWolf)
                {
                    description += ". " + design.Stats[St.DamageFlatModifier].Value + 
                                  " damage for each plank you have<size=100%>";
                }

                if (design.Category == DesignCategory.Ambush)
                {
                    description += ". Increase by " + design.GetStat(St.DamageFlatModifier) +
                                   " each turn it doesn't attack";
                }

                break;
            case DesignCategory.Ranged:
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
                break;
            case DesignCategory.Area:
                var distance = maxRange?.Value + " plank";
                if (maxRange?.Value != 1) distance += "s";
                description = "Attacks all enemies for " + damage?.Value + " damage when an enemy lands within " + distance +
                              " away ";
                break;
            case DesignCategory.Block: 
                description = "Removes " + block?.Value + " movement from enemies leaving this plank";
                break;
            case DesignCategory.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + boost?.Value;
                break;
            case DesignCategory.Hop:
                description = "Enemies leaving this plank skip the next plank";
                break;
            case DesignCategory.Reverse:
                description = "Reverses the direction ";
                if (design.Level == 0) description += "of ";
                else description += " and adds 1 movement to ";
                description += " an enemy leaving this plank";
                break;
            case DesignCategory.Poison:
                description = "Applies " + poison?.Value + " poison to enemies landing on this plank";
                break;
            case DesignCategory.StrikeZone:
                description = "Enemies on this plank take ";
                switch (design.Level)
                {
                    case 0:
                        description += "double damage from Attacks";
                        break;
                    case 1:
                        description += "double damage";
                        break;
                    case 2:
                        description += "triple damage";
                        break;
                }

                break;
            case DesignCategory.Infirmary:
                description = "At the end of the battle, recover " + healPlayer?.Value;
                if (healPlayer?.Value > 1) description += " lives";
                else description += " life";
                break;
            case DesignCategory.GrandFinalist:
                description = "When enemies land on this plank, deal " + damage?.Value +
                              " damage to all enemies. If any survive, lose a life";
                break;
            case DesignCategory.Cauterize:
                var multiplier = maxHealthMultiplier?.Value;
                var multiText = multiplier > 1 ? multiplier + "x " : "";
                description = "When enemies land on this plank, lower their maximum health by " + multiText +
                              "their poison amount";
                break;
        }

        if (design.Stats.TryGetValue(St.UsesPerTurn, out var usesPerTurn))
        {
            description += " (" + usesPerTurn?.Value + "x per turn)";
        }

        description = description.Replace("1x", "once");
        description = description.Replace("2x", "twice");
        description = description.Replace("3x", "triple");

        return description;
    }
}
