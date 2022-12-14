using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClausEnemy : Enemy
{
    protected override void Awake()
    {
        Name = "C'laus";
        MoveMax = 3;
        MoveMin = 2;
        MaxHealth = 10;
        Gold = 1;

        base.Awake();
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
