using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class CucungerEnemy : Enemy
{
    // Smashes the last stick every X turns
    private static int abilityCooldown = 2;
    [SerializeField] private int cooldownCounter = 0;

    protected override void Init()
    {
        Size = 1.2f;
        Name = "Cucunger";
        Mover.AddMove(1);
        SetInitialHealth(70);
        Gold = 10;
    }
    
    public override string GetDescription()
    {
        var turns = abilityCooldown - cooldownCounter;

        var turnText = "";
        switch (turns)
        {
            case 0:
                turnText = "this turn";
                break;
            case 1:
                turnText = "in 1 turn";
                break;
            default:
                turnText = "in " + turns + " turns";
                break;
        }
        
        return "Destroys the furthest plank to the right " + turnText;
    }

    protected override void StartOfTurnAbility()
    {
        var speakText = (abilityCooldown - cooldownCounter).ToString();
        if (speakText == "0")
            speakText = "X";

        Speak(speakText);
        
        if (cooldownCounter >= abilityCooldown && StickNum != StickManager.current.stickCount-1)
        {
            cooldownCounter = 0;
            StickManager.current.DestroyStick(StickManager.current.stickCount-1);
        }
        else
        {
            cooldownCounter++;
        }
        
        base.StartOfTurnAbility();
    }

    protected override bool TestForStartOfTurnAbility()
    {
        return true;
    }
}
