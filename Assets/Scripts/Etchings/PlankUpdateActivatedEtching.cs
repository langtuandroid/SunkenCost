using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using UnityEngine;

namespace Etchings
{
    public abstract class StickMovementActivatedEtching : Etching
    {
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            return TestStickMovementActivatedEffect(battleEvent);
        }
        
        protected abstract bool TestStickMovementActivatedEffect(BattleEvent battleEvent);
    }
}

