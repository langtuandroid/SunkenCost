using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

public class EnemyStats
{
    private Stat _poisonStat = new Stat(0);
    private Enemy _enemy;
    public int Poison => _poisonStat.Value;

    public EnemyStats(Enemy enemy)
    {
        _enemy = enemy;
    }

    public BattleEvent AddPoison(int amount)
    {
        _poisonStat.AddModifier(new StatModifier(amount, StatModType.Flat));
        return new BattleEvent(BattleEventType.EnemyPoisoned) {enemyAffectee = _enemy, modifier = amount};
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
