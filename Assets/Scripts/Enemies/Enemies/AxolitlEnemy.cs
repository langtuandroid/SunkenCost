using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

public class AxolitlEnemy : Enemy, IStartOfTurnAbilityHolder
{
    private int _healingAmount = 3;
    
    protected override void Init()
    {
        Name = "Axolitl";
        Mover.AddMove(1);
        Mover.AddMove(2);
        SetInitialHealth(21);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Heals " + _healingAmount + " health each turn";
    }

    public bool GetIfUsingStartOfTurnAbility()
    {
        return Health < MaxHealth;
    }

    public BattleEventPackage GetStartOfTurnAbility()
    {
        return new BattleEventPackage(MaxHealth - _healingAmount > _healingAmount
            ? Heal(_healingAmount) : Heal(MaxHealth - Health));
    }
}
