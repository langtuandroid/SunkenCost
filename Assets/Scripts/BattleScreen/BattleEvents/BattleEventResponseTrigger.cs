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
            int responderID, Func<BattleEvent, BattleEventPackage> response, Func<BattleEvent, bool> condition = null)
        {
            this.battleEventType = battleEventType;
            this.responderID = responderID;
            condition ??= b => true;
            this.condition = condition;
            this.response = response;
        }
    }
    
    public class ActionTrigger : BattleEventResponseTrigger
    {
        public ActionTrigger(BattleEventType battleEventType, int responderID, 
            Action<BattleEvent> action, Func<BattleEvent, bool> condition = null) : base(battleEventType, responderID, 
            b => { action.Invoke(b); return BattleEventPackage.Empty; }, condition)
        {
        }
        
        public ActionTrigger(BattleEventType battleEventType, int responderID, 
            Action action, Func<BattleEvent, bool> condition = null) : base(battleEventType, responderID, 
            b => { action.Invoke(); return BattleEventPackage.Empty; }, condition)
        {
        }
    }
}