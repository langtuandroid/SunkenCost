using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class StrongClausEnemy : Enemy
{
    protected override void Awake()
    {
        Name = "C'laus";
        MoveSet.Add(2);
        MoveSet.Add(3);
        MaxHealth = 20;
        image.color = new Color(0.83f, 0.79f, 0f);
        Gold = 1;

        base.Awake();
    }
    
    public override string GetDescription()
    {
        return "Always moves";
    }
}
