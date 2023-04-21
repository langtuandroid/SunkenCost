using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

public class GrouldEnemy : Enemy, IStartOfTurnAbilityHolder
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

    public bool GetIfUsingStartOfTurnAbility()
    {
        return PlankNum != 0;
    }

    public List<BattleEvent> GetStartOfTurnAbility()
    {
        var response = new List<BattleEvent>();
        var deactivate = Plank.Etching.Deactivate(DamageSource.EnemyAbility);
        deactivate.enemyAffector = this;
        response.Add(deactivate);
        return response;
    }
}
