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
    Infirmary,
    LoneWolf
}

public class DesignManager : MonoBehaviour
{
    public static DesignManager current;
    private static readonly Dictionary<string, Type> DesignTypes = new Dictionary<string, Type>();
    
    private static readonly Dictionary<string, Type> EtchingTypes = new Dictionary<string, Type>();
    
    [SerializeField] private List<string> etchingTypes = new List<string>();
    [SerializeField] private List<Sprite> etchingSprites = new List<Sprite>();

    public static readonly List<string> CommonDesigns = new List<string>();
    public static readonly List<string> UncommonDesigns = new List<string>();
    public static readonly List<string> RareDesigns = new List<string>();

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
            DesignTypes.Add(designName ?? "ERROR", type);

            // Create instances of the design to create the table of rarities
            var newDesign = (Design) Activator.CreateInstance(type);
            var rarity = newDesign.GetStat(St.Rarity);

            switch (rarity)
            {
                case 1:
                    CommonDesigns.Add(designName);
                    break;
                case 2:
                    UncommonDesigns.Add(designName);
                    break;
                case 3:
                    RareDesigns.Add(designName);
                    break;
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
        var index = current.etchingTypes.IndexOf(designCategory.ToString());
        return current.etchingSprites[index];
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

        switch (design.Category)
        {
            case DesignCategory.Melee: case DesignCategory.LoneWolf:  
                description = "Attacks an enemy landing on this plank for\n" + damage?.Value + " damage";
                if (design.Category == DesignCategory.LoneWolf)
                    description += ". " + design.Stats[St.DamageFlatModifier].Value + " damage for each plank you have";
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
                description = "Removes " + block?.Value + " movement from an enemy leaving this plank";
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
                description = "Applies " + poison?.Value + " poison to an enemy landing on this plank";
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
        }

        if (design.Stats.TryGetValue(St.UsesPerTurn, out var usesPerTurn))
        {
            description += " " + usesPerTurn?.Value + "x per turn";
        }

        return description;
    }
}
