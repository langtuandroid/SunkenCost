using UnityEngine;

namespace Etchings
{
    public class HopEtching : CharMovementActivatedEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;
            var stickNum = Stick.GetStickNumber();

            if (currentEnemy.StickNum != stickNum + (1 * currentEnemy.LastDirection)) return false;
            Debug.Log(currentEnemy.StickNum);
            
            //currentEnemy.NextMove += currentEnemy.Direction * initialHopAmount;
            if (currentEnemy.StickNum <
                StickManager.current.stickCount - 1 + (currentEnemy.LastDirection * GetStatValue(St.Hop)))
            {
                currentEnemy.StickNum += currentEnemy.LastDirection * GetStatValue(St.Hop);
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
