using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    Poison
}

public class DesignManager : MonoBehaviour
{
    public static DesignManager current;
    private static readonly Dictionary<string, Type> DesignTypes = new Dictionary<string, Type>();
    
    private static readonly Dictionary<string, Type> EtchingTypes = new Dictionary<string, Type>();
    
    [SerializeField] private List<string> etchingTypes = new List<string>();
    [SerializeField] private List<Sprite> etchingSprites = new List<Sprite>();
    
    public static Dictionary<string, int> Rarities { get; } = new Dictionary<string, int>()
    {
        // Melee
        {"Stab", 0},
        
        // Ranged
        {"Slinger", 0},
        {"Archer", 0},
        {"Marksman", 1},
        
        // Area
        {"Stomp", 0},
        {"Splatter", 1},
        
        // Block
        {"Impede", 0},
        
        // Boost
        {"Boost", 0},
        
        /* Hop
        {"Hop", 0}, */
        
        {"Reverse", 1},
        {"Poison", 0}
    };

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        // If reloading
        if (EtchingTypes.Count > 0) return;
        
        // Get the Designs
        var designs =
            Assembly.GetAssembly(typeof(Design)).GetTypes().Where(t => t.IsSubclassOf(typeof(Design)));

        foreach (var type in designs)
        {
            var designName = type.FullName;
            DesignTypes.Add(designName ?? "WHAT?", type);
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
        var index = current.etchingTypes.IndexOf(designCategory.ToString());
        return current.etchingSprites[index];
    }

    public static string GetDescription(DesignCategory designCategory, int timesPerTurn = -1, int minDistance = -1, int maxDistance = -1, int amount = -1)
    {
        var description = "";

        switch (designCategory)
        {
            case DesignCategory.Melee: 
                description = "Attacks an enemy landing on this plank for\n" + amount + " damage "+ timesPerTurn + "x per turn";
                break;
            case DesignCategory.Ranged:
                var range = "";
                if (minDistance == maxDistance)
                {
                    range = minDistance.ToString() + " plank";
                    if (minDistance != 1) range += "s";
                }
                else
                {
                    range = minDistance + "-" + maxDistance + " planks";
                }
                
                description = "Attacks an enemy landing " + range + " away for\n" + amount + " damage "+ timesPerTurn + "x per turn";
                break;
            case DesignCategory.Area:
                var distance = maxDistance + " plank";
                if (maxDistance != 1) distance += "s";
                description = "Attacks every enemy landing within " + distance + " away for\n" + amount + " damage "+ timesPerTurn + "x per turn";
                break;
            case DesignCategory.Block: 
                description = "Removes movement of an enemy landing on this plank "+ timesPerTurn + "x per turn";
                break;
            case DesignCategory.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + amount;
                break;
            case DesignCategory.Hop:
                description = "Enemies leaving this plank skip the next plank";
                break;
            case DesignCategory.Reverse:
                description = "Reverses the direction of an enemy leaving this plank " + timesPerTurn + "x per turn";
                break;
            case DesignCategory.Poison:
                description = "Applies " + amount + " poison to an enemy landing on this plank " + timesPerTurn + "x per turn";
                break;
        }

        return description;
    }

    public static string GetDescription(DesignCategory designCategory, Dictionary<St, Stat> stats)
    {
        var description = "";
        stats.TryGetValue(St.UsesPerTurn, out var usesPerTurn);
        stats.TryGetValue(St.MinRange, out var minRange);
        stats.TryGetValue(St.MaxRange, out var maxRange);
        stats.TryGetValue(St.Damage, out var damage);
        stats.TryGetValue(St.Boost, out var boost);
        stats.TryGetValue(St.Poison, out var poison);

        switch (designCategory)
        {
            case DesignCategory.Melee: 
                description = "Attacks an enemy landing on this plank for\n" + damage?.Value + " damage "+ usesPerTurn?.Value + "x per turn";
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
                
                description = "Attacks an enemy landing " + range + " away for\n" + damage?.Value + " damage "+ usesPerTurn?.Value + "x per turn";
                break;
            case DesignCategory.Area:
                var distance = maxRange?.Value + " plank";
                if (maxRange?.Value != 1) distance += "s";
                description = "Attacks every enemy landing within " + distance + " away for\n" + damage?.Value + " damage "+ usesPerTurn?.Value + "x per turn";
                break;
            case DesignCategory.Block: 
                description = "Removes " + stats[St.Block].Value + " movement from an enemy leaving this plank "+ usesPerTurn?.Value + "x per turn";
                break;
            case DesignCategory.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + boost?.Value;
                break;
            case DesignCategory.Hop:
                description = "Enemies leaving this plank skip the next plank";
                break;
            case DesignCategory.Reverse:
                description = "Reverses the direction of an enemy leaving this plank " + usesPerTurn?.Value + "x per turn. No upgrades.";
                break;
            case DesignCategory.Poison:
                description = "Applies " + poison?.Value + " poison to an enemy landing on this plank " + usesPerTurn?.Value + "x per turn";
                break;
        }

        return description;
    }
}
