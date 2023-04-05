using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AxolitlEnemy : Enemy, IStartOfTurnAbilityHolder
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
        return "Heals " + _healingAmount + " health each turn";
    }
    
    public IEnumerator ExecuteStartOfTurnAbility()
    {
        // Only heal up to max heath
        if (Health >= MaxHealth.Value) yield break;
        
        if (MaxHealth.Value - _healingAmount > _healingAmount)
        {
            yield return StartCoroutine(Heal(_healingAmount));
        }
        else
        {
            yield return StartCoroutine(Heal(MaxHealth.Value - Health));
        }
    }
}
