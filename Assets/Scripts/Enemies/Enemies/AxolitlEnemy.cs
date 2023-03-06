using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AxolitlEnemy : Enemy
{
    private int _healingAmount = 3;
    
    protected override void Init()
    {
        Name = "Axolitl";
        MoveSet.Add(1);
        MoveSet.Add(2);
        SetInitialHealth(15);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Gains health each turn";
    }

    protected override void PreMovingAbility()
    {
        // Heal up to max heath
        if (Health < MaxHealth.Value)
        {
            if (MaxHealth.Value - _healingAmount > _healingAmount)
            {
                Heal(_healingAmount);
            }
            else
            {
                Heal(MaxHealth.Value - Health);
            }
        }
        
        base.PreMovingAbility();
    }

    protected override bool TestForPreMovingAbility()
    {
        return true;
    }
}
