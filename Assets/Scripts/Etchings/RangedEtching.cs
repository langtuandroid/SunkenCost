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
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            if (enemy.PlankNum < 0 || enemy.PlankNum > Board.Current.PlankCount) return false;
            return Math.Abs(enemy.PlankNum - PlankNum) >= MinRange &&
                   Math.Abs(enemy.PlankNum - PlankNum) <= MaxRange;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            return new DesignResponse(enemy.PlankNum, DamageEnemy(enemy));
        }
    }
}