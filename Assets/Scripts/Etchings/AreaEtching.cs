﻿using System;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class AreaEtching : DamageEtching
    {
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum != -1 && Math.Abs(enemy.PlankNum - PlankNum) <= MaxRange;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            var plankNums = new List<int>();
            for (var i = PlankNum - MaxRange; i <= PlankNum + MaxRange && i <= Board.Current.PlankCount + 1; i++)
            {
                if (i < 0) 
                    i = 0;
                plankNums.Add(i);
            }

            var enemies = EnemySequencer.Current.GetEnemiesOnPlanks(plankNums);

            return new DesignResponse(plankNums, enemies.Select(DamageEnemy).ToList());
        }
    }
}