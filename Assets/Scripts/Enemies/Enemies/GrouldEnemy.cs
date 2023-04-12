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
        Mover.AddMove(1);
        SetInitialHealth(25);
        Gold = 1;
    }
    
    public override string GetDescription()
    {
        return "Disables the plank it starts each turn on";
    }

    protected override void StartOfTurnAbility()
    {
        yield return StartCoroutine(Plank.Etching?.Deactivate(1));
        InGameSfxManager.current.Slimed();
    }

    protected override bool HasStartOfTurnAbility()
    {
        return (StickNum != 0);
    }
}
