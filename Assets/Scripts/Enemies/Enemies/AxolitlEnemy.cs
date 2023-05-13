using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using Stats;
using UnityEngine;

public class AxolitlEnemy : Enemy, IStartOfTurnAbilityHolder
{
    public override string GetDescription()
    {
        return "Heals " + stats.GetModifier(EnemyModifierType.Health) + " health each turn";
    }

    public bool GetIfUsingStartOfTurnAbility()
    {
        return Health < MaxHealth;
    }

    public BattleEventPackage GetStartOfTurnAbility()
    {
        var healingAmount = stats.GetModifier(EnemyModifierType.Health);
        
        return new BattleEventPackage(MaxHealth - healingAmount > healingAmount
            ? Heal(healingAmount) : Heal(MaxHealth - Health));
    }
}
