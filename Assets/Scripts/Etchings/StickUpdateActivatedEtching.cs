using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etchings
{
    public abstract class StickUpdateActivatedEtching : ActiveEtching
    {
        public void DetectStickMovement()
        {
            /* Deactivated
            if (deactivationTurns > 0) return;

            if (UsesUsedThisTurn >= UsesPerTurn && !limitless) return;
            */
            
            if (!TestStickMovementActivatedEffect()) return;
            
            designInfo.RefreshCard();
            if (colorWhenActivated) StartCoroutine(ColorForActivate());
        }
        
        protected abstract bool TestStickMovementActivatedEffect();
    }
}

