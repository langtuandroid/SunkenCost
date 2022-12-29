using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etchings
{
    public abstract class CharMovementActivatedEtching : ActiveEtching
    {
        public bool DetectCharacterMovement()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;
            
            // Deactivated
            if (deactivationTurns > 0) return false;
            
            // GAMEOVER?
            if (enemy.StickNum >= StickManager.current.stickCount + 1) return false;
            
            if ((UsesUsedThisTurn < UsesPerTurn || design.Limitless) && TestCharMovementActivatedEffect())
            {
                StartCoroutine(ColorForActivate());
                Log.current.AddEvent(design.Title + " on S" + Stick.GetStickNumber() + " activates against E" + enemy.name +
                                     " on S" + enemy.StickNum);
                
                designInfo.Refresh();
                return true;
            }

            return false;
        }

        protected abstract bool TestCharMovementActivatedEffect();
    } 
}

