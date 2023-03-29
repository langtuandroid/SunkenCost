using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AxolitlEnemy : Enemy
{
    private int _healingAmount = 5;
    
    protected override void Init()
    {
        Name = "Axolitl";
        Mover.AddMove(1);
        Mover.AddMove(2);
        SetInitialHealth(25);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Gains health each turn";
    }

    protected override void StartOfTurnAbility()
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
        
        base.StartOfTurnAbility();
    }

    protected override bool TestForStartOfTurnAbility()
    {
        return true;
    }
}
