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
        _enemy.UI.PoisonImage.enabled = true;
        _enemy.UI.PoisonText.enabled = true;
        _enemy.UI.PoisonText.text = Poison.ToString();
        return new BattleEvent(BattleEventType.EnemyPoisoned) {enemyAffectee = _enemy, modifier = amount};
    }

    public void RemovePoison(int amount)
    {
        _poisonStat.AddModifier(new StatModifier(-amount, StatModType.Flat));
        _enemy.UI.PoisonText.text = Poison.ToString();

        if (Poison <= 0)
        {
            _enemy.UI.PoisonImage.enabled = false;
            _enemy.UI.PoisonText.enabled = false;
            _poisonStat = new Stat();
        }
    }
}
