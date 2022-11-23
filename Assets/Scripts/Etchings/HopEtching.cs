using UnityEngine;

namespace Etchings
{
    public class HopEtching : CharMovementActivatedEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.current.CurrentEnemy;
            var stickNum = Stick.GetStickNumber();

            if (currentEnemy.StickNum != stickNum + (1 * currentEnemy.Direction)) return false;
            Debug.Log(currentEnemy.StickNum);
            
            //currentEnemy.NextMove += currentEnemy.Direction * initialHopAmount;
            if (currentEnemy.StickNum <
                StickManager.current.stickCount - 1 + (currentEnemy.Direction * GetStatValue(St.Hop)))
            {
                currentEnemy.StickNum += currentEnemy.Direction * GetStatValue(St.Hop);
            }

            UsesUsedThisTurn += 1;
            return true;

        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }
    }
}
