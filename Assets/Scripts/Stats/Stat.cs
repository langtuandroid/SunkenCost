using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Stat
{
    private float _lastBaseValue = float.MinValue;

    private bool _needsRecalculating = true;
    private int _value;
    
    private readonly List<StatModifier> _statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    

    public int Value
    {
        get
        {
            if (!_needsRecalculating && !(Math.Abs(BaseValue - _lastBaseValue) > 0.01f)) return _value;
            _lastBaseValue = BaseValue;
            _value = CalculateFinalValue();
            _needsRecalculating = false;

            return _value;
        }
    }

    public float BaseValue { get; private set; }

    public Stat()
    {
        _statModifiers = new List<StatModifier>();
        StatModifiers = _statModifiers.AsReadOnly();
    }
    
    public Stat(int baseValue) : this()
    {
        BaseValue = baseValue;
        
    }

    public void ModifyBaseValue(int amount)
    {
        BaseValue += amount;
        _needsRecalculating = true;
    }

    public void AddModifier(StatModifier mod)
    {
        _needsRecalculating = true;
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
            _needsRecalculating = true;
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
                _needsRecalculating = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    private int CalculateFinalValue()
    {
        var finalValue = BaseValue;
        float sumPercentAdd = 0;

        // If there's override stat mod types, return the largest numbered override
        var overrides = _statModifiers.Where(sm => sm.Type == StatModType.Override).ToList();
        if (overrides.Any())
        {
            return (int)overrides.Max(sm => sm.Value);
        }

        for (var i = 0; i < _statModifiers.Count; i++)
        {
            var statModifier = _statModifiers[i];

            switch (statModifier.Type)
            {
                case StatModType.Flat:
                    finalValue += statModifier.Value;
                    break;
                case StatModType.PercentAdd:
                {
                    sumPercentAdd += statModifier.Value;

                    if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }

                    break;
                }
                case StatModType.PercentMult:
                    finalValue *= 1 + statModifier.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return (int) Math.Round(finalValue, 0);
    }
}
