using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class GrouldEnemy : Enemy
{
    protected override void Init()
    {
        Name = "Grould";
        MoveSet.Add(1);
        SetInitialHealth(20);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Disables the plank it starts each turn on";
    }

    protected override void PreMovingAbility()
    {
        Stick.etching?.Deactivate(1);
        InGameSfxManager.current.Slimed();
    }

    protected override bool TestForPreMovingAbility()
    {
        return (StickNum != 0);
    }
}
