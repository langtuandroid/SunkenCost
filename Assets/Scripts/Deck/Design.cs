using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum St
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
    DamageFlatModifier
}

public abstract class Design
{
    public string Title { get; protected set; }
    public DesignCategory Category { get; protected set; }
    
    public Color Color { get; protected set; }
    public bool Limitless { get; private set; }
    public int Level { get; private set; }
    public bool Upgradeable { get; protected set; } = true;

    public int UsesUsedThisTurn;

    public Dictionary<St, Stat> Stats { get; private set; } = new Dictionary<St, Stat>();

    protected Design()
    {
        Init();
    }

    protected virtual void Init()
    {
        // Limitless
        if (!Stats.ContainsKey(St.UsesPerTurn))
        {
            Limitless = true;
        }
    }

    protected void AddStat(St st, int value)
    {
        Stats.Add(st, new Stat(value));
    }

    protected void ModifyStat(St st, int value)
    {
        Stats[st].AddModifier(new StatModifier(value, StatModType.Flat));
    }

    public int GetStat(St st)
    {
        Stats.TryGetValue(st, out var stat);
        if (stat != null) return stat.Value;
        return -1;
    }

    public void LevelUp()
    {
        if (!Upgradeable || Level >= 2) return;
        Level++;

        if (Level % 2 == 0) SecondLevelUp();
        else FirstLevelUp();
    }

    protected abstract void FirstLevelUp();
    protected abstract void SecondLevelUp();
}