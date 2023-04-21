using System;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class AreaEtching : DamageEtching
    {
        //TEST INFLUENCE
        //return Math.Abs(PlankNum - battleEvent.enemyAffected.PlankNum) <= MaxRange;

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var plankNums = new List<int>();
            for (var i = PlankNum - MaxRange; i <= PlankNum + MaxRange && i < PlankMap.Current.PlankCount; i++)
            {
                if (i <= 0) 
                    i = 1;
                plankNums.Add(i);
            }

            var enemies = EnemyController.Current.GetEnemiesOnPlanks(plankNums);

            return enemies.Select(DamageEnemy).ToList();
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            if (Math.Abs(enemy.PlankNum - PlankNum) > MaxRange) return false;
            UsesUsedThisTurn++;
            return true;
        }
    }
}