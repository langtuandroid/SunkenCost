using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public abstract class BattleEventResponder : MonoBehaviour
    {
        public abstract bool GetIfRespondingToBattleEvent(BattleEvent battleEvent);
        
        public virtual List<BattleEvent> GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            var response = GetResponse(battleEvent);
            return BattleEventsManager.Current.GetEventAndResponsesList(response);
        }

        protected virtual BattleEvent GetResponse(BattleEvent battleEvent)
        {
            throw new NotImplementedException();
        }
    }
}