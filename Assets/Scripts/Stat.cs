using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Stat
{
    private float _baseValue;
    private float lastBaseValue = float.MinValue;

    private bool needsRecalculating = true;
    private int _value;
    
    private readonly List<StatModifier> _statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    

    public int Value
    {
        get
        {
            if (needsRecalculating || _baseValue != lastBaseValue)
            {
                lastBaseValue = _baseValue;
                _value = (int) CalculateFinalValue();
                needsRecalculating = false;
            }

            return _value;
        }
    }

    public float BaseValue => _baseValue;

    public Stat()
    {
        _statModifiers = new List<StatModifier>();
        StatModifiers = _statModifiers.AsReadOnly();
    }
    
    public Stat(int baseValue) : this()
    {
        _baseValue = baseValue;
        
    }

    public void AddModifier(StatModifier mod)
    {
        needsRecalculating = true;
        _statModifiers.Add(mod);
        _statModifiers.Sort(CompareModifierOrder);
    }

    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (_statModifiers.Remove(mod))
        {
            needsRecalculating = true;
            return true;
        }

        return false;
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        var didRemove = false;
        
        for (var i = _statModifiers.Count - 1; i >= 0; i++)
        {
            if (_statModifiers[i].Source == source)
            {
                needsRecalculating = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    private int CalculateFinalValue()
    {
        var finalValue = _baseValue;
        float sumPercentAdd = 0;

        for (var i = 0; i < _statModifiers.Count; i++)
        {
            StatModifier statModifier = _statModifiers[i];
            
            if (statModifier.Type == StatModType.Flat)
            {
                finalValue += statModifier.Value;
            }
            else if (statModifier.Type == StatModType.PercentAdd)
            {
                sumPercentAdd += statModifier.Value;

                if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (statModifier.Type == StatModType.PercentMult)
            {
                finalValue *= 1 + statModifier.Value;
            }
            
        }

        return (int) Math.Round(finalValue, 0);
    }
}
