using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using Stats;
using UnityEngine;

public class EnemyStats
{
    private readonly int _enemyResponderID;
    
    private Stat _poisonStat = new Stat(0);

    private readonly Dictionary<EnemyModifierType, int> _abilityModifiers;
    
    public int Poison => _poisonStat.Value;

    public EnemyStats(int enemyResponderID, Dictionary<EnemyModifierType, int> abilityModifiers)
    {
        _abilityModifiers = abilityModifiers;
        _enemyResponderID = enemyResponderID;
    }

    public BattleEvent AddPoison(int amount)
    {
        _poisonStat.AddModifier(new StatModifier(amount, StatModType.Flat));
        return new BattleEvent(BattleEventType.EnemyPoisoned) {creatorID = _enemyResponderID, modifier = amount};
    }

    /*
    public BattleEvent RemovePoison(int amount)
    {
        _poisonStat.AddModifier(new StatModifier(-amount, StatModType.Flat));
        if (Poison <= 0)
        {
            _poisonStat = new Stat();
        }
    }
    */

    public int GetModifier(EnemyModifierType modifierType)
    {
        if (_abilityModifiers.TryGetValue(modifierType, out var modifier))
        {
            return modifier;
        }

        var enemyName = BattleEventResponseSequencer.Current.GetEnemyByResponderID(_enemyResponderID).Name;
        throw new Exception(enemyName + " has no modifier of type " + modifierType);
    }
}
