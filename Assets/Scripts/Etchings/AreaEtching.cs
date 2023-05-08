using System;
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
        //TEST INFLUENCE
        //return Math.Abs(PlankNum - battleEvent.enemyAffected.PlankNum) <= MaxRange;
        
        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return Math.Abs(enemy.PlankNum - PlankNum) <= MaxRange;
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var plankNums = new List<int>();
            for (var i = PlankNum - MaxRange; i <= PlankNum + MaxRange && i <= Board.Current.PlankCount + 1; i++)
            {
                if (i < 0) 
                    i = 0;
                plankNums.Add(i);
            }

            var enemies = EnemySequencer.Current.GetEnemiesOnPlanks(plankNums);

            return new DesignResponse(plankNums, enemies.Select(e => DamageEnemy(e.ResponderID)).ToList());
        }
    }
}