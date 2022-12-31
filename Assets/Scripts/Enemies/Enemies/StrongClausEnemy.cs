using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class StrongClausEnemy : Enemy
{
    protected override void Init()
    {
        Name = "Strong C'laus";
        MoveSet.Add(2);
        MoveSet.Add(3);
        SetInitialHealth(20);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
