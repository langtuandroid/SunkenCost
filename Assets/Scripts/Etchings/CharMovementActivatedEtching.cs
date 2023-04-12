using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etchings
{
    public abstract class CharMovementActivatedEtching : Etching
    {
        public bool DetectCharacterMovement()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;
            
            // Deactivated
            if (deactivationTurns > 0) return false;
            
            // GAMEOVER?
            if (enemy.StickNum >= PlankMap.current.stickCount + 1) return false;
            
            if ((design.Limitless || UsesUsedThisTurn < UsesPerTurn) && TestCharMovementActivatedEffect())
            {
                StartCoroutine(ColorForActivate());
                Log.current.AddEvent(design.Title + " on S" + Plank.GetPlankNum() + " activates against E" + enemy.name +
                                     " on S" + enemy.StickNum);
                
                etchingDisplay.UpdateDisplay();
                return true;
            }

            return false;
        }

        protected abstract bool TestCharMovementActivatedEffect();
    } 
}

