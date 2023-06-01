using System;

namespace BattleScreen.BattleEvents
{
    public class BattleEventResponseTrigger
    {
        public readonly BattleEventType battleEventType;
        public readonly int responderID;
        public readonly Func<BattleEvent, bool> condition;
        public readonly Func<BattleEvent, BattleEventPackage> response;

        public BattleEventResponseTrigger(BattleEventType battleEventType, 
            int responderID, Func<BattleEvent, bool> condition, Func<BattleEvent, BattleEventPackage> response)
        {
            this.battleEventType = battleEventType;
            this.responderID = responderID;
            this.condition = condition;
            this.response = response;
        }
    }
}