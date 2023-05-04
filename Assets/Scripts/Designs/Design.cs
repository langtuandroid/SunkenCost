using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Designs
{
    public enum StatType
    {
        Rarity,
        UsesPerTurn,
        Damage,
        MinRange,
        MaxRange,
        Boost,
        Poison,
        Hop,
        Block,
        StatMultiplier,
        HealPlayer,
        DamageFlatModifier,
        MovementBoost,
        ModifyPlayerHealth,
        HealEnemy,
        Gold,
        IntRequirement
    }

    public class Design
    {
        public readonly DesignAsset designAsset;

        public string Title => designAsset.title;
        public Sprite Sprite => designAsset.sprite;
        public DesignType Type => designAsset.designType;

        public Color Color => designAsset._color;
        public bool Limitless { get; private set; }
        public int Level { get; private set; }
        public bool Upgradeable { get; protected set; } = true;

        public int UsesUsedThisTurn;

        public int Cost { get; private set; }

        public Dictionary<StatType, Stat> Stats { get; private set; } = new Dictionary<StatType, Stat>();

        public Design(DesignAsset designAsset)
        {
            this.designAsset = designAsset;
        
            foreach (var statSetter in designAsset.initialStats)
            {
                AddStat(statSetter.StatType, statSetter.Magnitude);
            }
        
            // Limitless
            if (!Stats.ContainsKey(StatType.UsesPerTurn))
            {
                Limitless = true;
            }
        
            Cost = RunProgress.PlayerStats.PriceHandler.GetCardCost(this.designAsset.rarity);
        }

        public int GetStat(StatType statType)
        {
            Stats.TryGetValue(statType, out var stat);
            if (stat != null) return stat.Value;
        
            Debug.Log("Stat " + statType + " not found on design " + Title);
            return -1;
        }

        public void LevelUp()
        {
            if (!Upgradeable || Level >= 2) return;
            Level++;

            var levelUpStatMods = Level % 2 == 1 
                ? designAsset.firstLevelUpStatMods : designAsset.secondLevelUpStatMods;
            
        
            foreach (var statSetter in levelUpStatMods)
            {
                if (Stats.ContainsKey(statSetter.StatType))
                {
                    ModifyStat(statSetter.StatType, statSetter.Magnitude);
                }
                else
                {
                    AddStat(statSetter.StatType, statSetter.Magnitude);
                }
            }
        }

        public void SetCost(int cost)
        {
            Cost = cost;
        }

        private void AddStat(StatType statType, int value) 
        {
            Stats.Add(statType, new Stat(value));
        }

        private void ModifyStat(StatType statType, int value)
        {
            Stats[statType].ModifyBaseValue(value);
        }
    }
}