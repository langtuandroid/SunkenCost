using System;
using UnityEngine;

namespace Etchings
{
    public class ReverseEtching : CharPreMovementActivatedEffect
    {
        protected override bool TestCharAboutToMoveActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;
            var stickNum = Stick.GetStickNumber();
            var currentEnemyStickNum = currentEnemy.StickNum;

            if (currentEnemyStickNum != stickNum || currentEnemy.NextMove == 0) return false;

            // Set the new goal stick as the opposite direction
            currentEnemy.NextMove *= -1;

            currentEnemy.NextMove += GetStatValue(St.MovementBoost) * currentEnemy.NextDirection;
            
            currentEnemy.UpdateMovementText();

            if (currentEnemy.NextMove <= -currentEnemyStickNum) currentEnemy.NextMove = -currentEnemyStickNum;

            UsesUsedThisTurn += 1;
            return true;

        }
        
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }
    }
}
