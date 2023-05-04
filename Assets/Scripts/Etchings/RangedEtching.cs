using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class RangedEtching : DamageEtching
    {
        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            if (enemy.PlankNum < 0 || enemy.PlankNum > Board.Current.PlankCount) return false;
            return Math.Abs(enemy.PlankNum - PlankNum) >= MinRange &&
                   Math.Abs(enemy.PlankNum - PlankNum) <= MaxRange;
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            return new DesignResponse(enemy.PlankNum, DamageEnemy(battleEvent.affectedResponderID));
        }
    }
}