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
    Poison,
    StrikeZone,
    Infirmary
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
        {"Swordsman", 0},
        
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
        {"StrikeZone", 0},
        
        /* Hop
        {"Hop", 0}, */
        
        {"Reverse", 1},
        {"Poison", 0},
        {"Infirmary", 1}
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

    public static string GetDescription(DesignCategory designCategory, Dictionary<St, Stat> stats, int level = 0)
    {
        var description = "";
        stats.TryGetValue(St.MinRange, out var minRange);
        stats.TryGetValue(St.MaxRange, out var maxRange);
        stats.TryGetValue(St.Damage, out var damage);
        stats.TryGetValue(St.Boost, out var boost);
        stats.TryGetValue(St.Poison, out var poison);
        stats.TryGetValue(St.HealPlayer, out var healPlayer);

        switch (designCategory)
        {
            case DesignCategory.Melee: 
                description = "Attacks an enemy landing on this plank for\n" + damage?.Value + " damage";
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
                
                description = "Attacks an enemy landing " + range + " away for\n" + damage?.Value + " damage";
                break;
            case DesignCategory.Area:
                var distance = maxRange?.Value + " plank";
                if (maxRange?.Value != 1) distance += "s";
                description = "Attacks every enemy landing within " + distance + " away for\n" + damage?.Value + " damage";
                break;
            case DesignCategory.Block: 
                description = "Removes " + stats[St.Block].Value + " movement from an enemy leaving this plank";
                break;
            case DesignCategory.Boost: 
                description = "Boosts damage of adjacent Attack planks by " + boost?.Value;
                break;
            case DesignCategory.Hop:
                description = "Enemies leaving this plank skip the next plank";
                break;
            case DesignCategory.Reverse:
                description = "Reverses the direction ";
                if (level == 0) description += "of ";
                else description += " and adds 1 movement to ";
                description += " an enemy leaving this plank";
                break;
            case DesignCategory.Poison:
                description = "Applies " + poison?.Value + " poison to an enemy landing on this plank";
                break;
            case DesignCategory.StrikeZone:
                description = "Enemies on this plank take ";
                switch (level)
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
                description = "At the end of the battle, recover " + healPlayer?.Value + " life";
                break;
        }

        if (stats.TryGetValue(St.UsesPerTurn, out var usesPerTurn))
        {
            description += " " + usesPerTurn?.Value + "x per turn";
        }

        return description;
    }
}
