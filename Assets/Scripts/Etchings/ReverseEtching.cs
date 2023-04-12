using System;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class ReverseEtching : CharPreMovementActivatedEffect
    {
        protected override bool TestCharAboutToMoveActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;
            var stickNum = Plank.GetPlankNum();
            var currentEnemyStickNum = currentEnemy.StickNum;

            if (currentEnemyStickNum != stickNum || currentEnemy.FinishedMoving) return false;

            // Set the new goal stick as the opposite direction
            currentEnemy.Mover.Reverse();

            currentEnemy.Mover.AddMovement(GetStatValue(StatType.MovementBoost));

            UsesUsedThisTurn += 1;
            return true;

        }
        
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Plank.GetPlankNum();
        }
    }
}
