using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

public class CucungerEnemy : Enemy, IStartOfTurnAbilityHolder
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

    public bool GetIfUsingStartOfTurnAbility()
    {
        return true;
    }

    public BattleEventPackage GetStartOfTurnAbility()
    {
        var response = new List<BattleEvent>();
        
        var speech = (abilityCooldown - cooldownCounter).ToString();
        if (speech == "0")
            speech = "X";
        
        response.Add(Speak(speech));
        
        if (cooldownCounter >= abilityCooldown && PlankNum != Board.Current.PlankCount - 1)
        {
            cooldownCounter = 0;
            response.Add(PlankFactory.Current.DestroyPlank
                (DamageSource.EnemyAbility, Board.Current.PlankCount - 1));
        }
        else
        {
            cooldownCounter++;
        }

        return new BattleEventPackage(response);
    }
}
