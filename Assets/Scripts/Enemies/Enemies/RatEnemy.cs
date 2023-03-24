using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class RatEnemy : Enemy
{
    protected override void Init()
    {
        Name = "Scrat";
        MoveSet.Add(2);
        SetInitialHealth(10);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
