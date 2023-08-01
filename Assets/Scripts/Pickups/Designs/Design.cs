using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Pickups.Varnishes;
using UnityEngine;
using Varnishes;

namespace Designs
{
    public enum StatType
    {
        SOMETHINGELSEHERE0,
        UsesPerTurn,
        Damage,
        MinRange,
        MaxRange,
        StatFlatModifier,
        Poison,
        Hop,
        Block,
        StatMultiplier,
        SOMETHINGELSEHERE,
        SOMETHINGELSEHERE2,
        MovementBoost,
        PlayerHealthModifier,
        SOMETHINGELSEHERE3,
        Gold,
        IntRequirement
    }

    public class Design
    {
        public readonly DesignAsset designAsset;

        public int UsesUsedThisTurn { get; set; }
        public string Title => designAsset.title;
        public Sprite Sprite => designAsset.sprite;
        public DesignType Type => designAsset.designType;
        public DesignCategory Category => designAsset.designCategory;

        public Color Color => designAsset._color;
        public bool Limitless { get; private set; }
        public int Level { get; private set; }
        public bool Upgradeable { get; protected set; } = true;

        public int Cost { get; private set; }
        
        public List<VarnishType> Varnishes { get; } = new List<VarnishType>();

        private readonly Dictionary<StatType, Stat> _stats = new Dictionary<StatType, Stat>();

        private readonly Dictionary<StatModifier, StatType> _temporaryStatMods =
            new Dictionary<StatModifier, StatType>();

        public Design(DesignAsset designAsset)
        {
            this.designAsset = designAsset;
        
            foreach (var statSetter in designAsset.initialStats)
            {
                AddStat(statSetter.StatType, statSetter.Magnitude);
            }
        
            // Limitless
            if (!_stats.ContainsKey(StatType.UsesPerTurn))
            {
                Limitless = true;
            }
        
            Cost = RunProgress.Current.PlayerStats.PriceHandler.GetCardCost(this.designAsset.rarity);
        }

        public void LevelUp()
        {
            if (!Upgradeable || Level >= 2) return;
            Level++;

            var levelUpStatMods = Level % 2 == 1 
                ? designAsset.firstLevelUpStatMods : designAsset.secondLevelUpStatMods;
            
        
            foreach (var statSetter in levelUpStatMods)
            {
                if (_stats.ContainsKey(statSetter.StatType))
                {
                    ModifyStatBaseValue(statSetter.StatType, statSetter.Magnitude);
                }
                else
                {
                    AddStat(statSetter.StatType, statSetter.Magnitude);
                }
            }
        }
        
        public bool HasStat(StatType statType) => _stats.ContainsKey(statType);

        public int GetStat(StatType statType)
        {
            if (_stats.TryGetValue(statType, out var stat)) return stat.Value;
        
            Debug.Log("Stat " + statType + " not found on design " + Title);
            throw new UnexpectedStatException(statType);
        }
        
        public int GetStatBase(StatType statType)
        {
            if (_stats.TryGetValue(statType, out var stat)) return (int)stat.BaseValue;
        
            Debug.Log("Stat " + statType + " not found on design " + Title);
            throw new UnexpectedStatException(statType);
        }

        public void AddTempStatModifier(StatType statType, StatModifier statMod)
        {
            if (!HasStat(statType)) throw new UnexpectedStatException(statType);
            _stats[statType].AddModifier(statMod);
            
            _temporaryStatMods.Add(statMod, statType);
        }
        
        public void RemoveTempStatModifier(StatType statType, StatModifier statMod)
        {
            if (!HasStat(statType)) throw new UnexpectedStatException(statType);
            _stats[statType].RemoveModifier(statMod);
            
            _temporaryStatMods.Remove(statMod);
        }

        public void RemoveAllTempStatModifiers()
        {
            foreach (var (statMod, statType) in _temporaryStatMods)
            {
                _stats[statType].RemoveModifier(statMod);
            }
        }

        public void SetCost(int cost)
        {
            Cost = cost;
        }

        public void AddVarnish(VarnishType varnishType)
        {
            Varnishes.Add(varnishType);
        }

        private void AddStat(StatType statType, int value) 
        {
            _stats.Add(statType, new Stat(value));
        }

        private void ModifyStatBaseValue(StatType statType, int value)
        {
            _stats[statType].ModifyBaseValue(value);
        }
    }
}