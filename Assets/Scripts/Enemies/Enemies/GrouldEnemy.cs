using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using UnityEngine;

public class GrouldEnemy : Enemy, IStartOfTurnAbilityHolder
{
    public override string GetDescription()
    {
        return "Disables the plank it starts each turn on";
    }

    public bool GetIfUsingStartOfTurnAbility()
    {
        return PlankNum != -1;
    }

    public BattleEventPackage GetStartOfTurnAbility()
    {
        var stun = Plank.Etching.Stun(DamageSource.EnemyAbility);
        stun.secondaryResponderID = ResponderID;
        return new BattleEventPackage(stun);
    }
}
