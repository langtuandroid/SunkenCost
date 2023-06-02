using System;

namespace BattleScreen.BattleEvents
{
    public class BattleEventResponseTrigger
    {
        public readonly BattleEventType battleEventType;
        public readonly Func<BattleEvent, bool> condition;
        public readonly Func<BattleEvent, BattleEventPackage> response;

        public BattleEventResponseTrigger(BattleEventType battleEventType, Func<BattleEvent, BattleEventPackage> response,
            Func<BattleEvent, bool> condition = null)
        {
            this.battleEventType = battleEventType;
            condition ??= b => true;
            this.condition = condition;
            this.response = response;
        }

        public BattleEventResponseTrigger(BattleEventType battleEventType, Func<BattleEvent, BattleEvent> response,
            Func<BattleEvent, bool> condition = null) : this(battleEventType,
            b => new BattleEventPackage(response.Invoke(b)), condition)
        {
            
        }
    }
    
    public class ActionTrigger : BattleEventResponseTrigger
    {
        public ActionTrigger(BattleEventType battleEventType, Action<BattleEvent> action, 
            Func<BattleEvent, bool> condition = null) : base(battleEventType,
            b => { action.Invoke(b); return BattleEventPackage.Empty; }, condition)
        {
        }
        
        public ActionTrigger(BattleEventType battleEventType, Action action, 
            Func<BattleEvent, bool> condition = null) : base(battleEventType,
            b => { action.Invoke(); return BattleEventPackage.Empty; }, condition)
        {
        }
    }
}