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
        return PlankNum != -1;
    }

    public List<BattleEvent> GetStartOfTurnAbility()
    {
        var response = new List<BattleEvent>();
        var stun = Plank.Etching.Stun(DamageSource.EnemyAbility);
        stun.enemyAffector = this;
        response.Add(stun);
        return response;
    }
}
