using System.Collections;
using System.Collections.Generic;
using Etchings;
using UnityEngine;

public abstract class DamageEtching : CharMovementActivatedEtching
{
    protected int Damage => GetStatValue(St.Damage);
    protected int MinRange => GetStatValue(St.MinRange);
    protected int MaxRange => GetStatValue(St.MaxRange);

    protected virtual void DamageEnemy(Enemy enemy)
    {
        enemy.Damage(Damage, DamageSource.Plank);
    }

    public void ModifyStat(St stat, int amount)
    {
        if (!design.Stats.ContainsKey(stat)) return;
        
        design.Stats[stat].AddModifier(new StatModifier(amount, StatModType.Flat));
        designInfo.Refresh();
    }
}
