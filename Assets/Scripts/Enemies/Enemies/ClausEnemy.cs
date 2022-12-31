using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class ClausEnemy : Enemy
{
    protected override void Init()
    {
        Name = "C'laus";
        MoveSet.Add(2);
        MoveSet.Add(3);
        SetInitialHealth(16);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
