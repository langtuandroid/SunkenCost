using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClausEnemy : Enemy
{
    protected override void Awake()
    {
        Name = "C'laus";
        MoveSet.Add(2);
        MoveSet.Add(3);
        MaxHealth = 10;
        Gold = 1;

        base.Awake();
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
