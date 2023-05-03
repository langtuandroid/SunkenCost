using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

public class EnemyStats
{
    private readonly int _enemyResponderID;
    
    private Stat _poisonStat = new Stat(0);
    public int Poison => _poisonStat.Value;

    public EnemyStats(int enemyResponderID)
    {
        _enemyResponderID = enemyResponderID;
    }

    public BattleEvent AddPoison(int amount)
    {
        _poisonStat.AddModifier(new StatModifier(amount, StatModType.Flat));
        return new BattleEvent(BattleEventType.EnemyPoisoned) {affectedResponderID = _enemyResponderID, modifier = amount};
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
}
