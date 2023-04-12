using Designs;
using UnityEngine;

namespace Etchings
{
    public class HopEtching : CharMovementActivatedEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;
            var stickNum = Plank.GetPlankNum();

            if (currentEnemy.FinishedMoving) return false;
            
            // Check if leaving this etching

            if (currentEnemy.StickNum - currentEnemy.Mover.LastDirection != stickNum) return false;
            
            currentEnemy.Mover.AddSkip(GetStatValue(StatType.Hop));
            StartCoroutine(ColorForActivate());

            UsesUsedThisTurn += 1;
            return true;

        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Plank.GetPlankNum();
        }
    }
}