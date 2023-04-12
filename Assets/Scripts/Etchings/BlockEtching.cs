using Designs;
using UnityEngine;

namespace Etchings
{
    public class BlockEtching : CharPreMovementActivatedEffect
    {
        protected override bool TestCharAboutToMoveActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;
            var stickNum = Plank.GetPlankNum();
            var currentEnemyStickNum = currentEnemy.StickNum;

            if (currentEnemyStickNum != stickNum || currentEnemy.FinishedMoving) return false;
            
            currentEnemy.Block(design.GetStat(StatType.Block));
            UsesUsedThisTurn += 1;
            return true;

        }
        
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Plank.GetPlankNum();
        }
    }
}