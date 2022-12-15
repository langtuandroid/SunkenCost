using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrouldEnemy : Enemy
{
    protected override void Awake()
    {
        Name = "Grould";
        MoveSet.Add(1);
        MoveSet.Add(0);
        MaxHealth = 12;
        Gold = 1;

        base.Awake();
    }
    
    public override string GetDescription()
    {
        return "Disables the plank it's on when it doesn't move";
    }

    protected override void PreMovingAbility()
    {
        Stick.etching?.Deactivate(1);
        InGameSfxManager.current.Slimed();
    }

    protected override bool TestForPreMovingAbility()
    {
        // Only if not moving and not on startstick
        return (NextMove == 0 && StickNum != 0);
    }
}
