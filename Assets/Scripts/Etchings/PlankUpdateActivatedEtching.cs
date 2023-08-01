using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using UnityEngine;

namespace Etchings
{
    public abstract class PlankUpdateActivatedEtching : Etching
    {
        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            return new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(BattleEventType.StartedNextPlayerTurn, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.ReDrewPlanks, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.EndedBattle, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.PlankDestroyed, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.PlankMoved, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.EtchingsOrderChanged, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.PlankCreated, GetPlankUpdateResponse),
                new DesignResponseTrigger(BattleEventType.DesignModified, GetPlankUpdateResponse, GetIfThisIsPrimaryResponder),
            };
        }

        protected abstract DesignResponse GetPlankUpdateResponse(BattleEvent b);
    }
}

