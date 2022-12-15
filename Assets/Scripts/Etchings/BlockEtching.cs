using UnityEngine;

namespace Etchings
{
    public class BlockEtching : CharPreMovementActivatedEffect
    {
        protected override bool TestCharAboutToMoveActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.current.CurrentEnemy;
            var stickNum = Stick.GetStickNumber();
            var currentEnemyStickNum = currentEnemy.StickNum;

            if (currentEnemyStickNum != stickNum || currentEnemy.NextMove == 0) return false;
            
            currentEnemy.Block(design.GetStat(St.Block));
            UsesUsedThisTurn += 1;
            return true;

        }
        
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }
    }
}