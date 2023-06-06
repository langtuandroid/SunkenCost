using System;
using System.Collections.Generic;

namespace BattleScreen.BattleEvents
{
    public class BattleEventTrigger
    {
        public readonly BattleEventType battleEventType;
        public readonly Func<BattleEvent, bool> condition;

        public BattleEventTrigger(BattleEventType battleEventType, 
            Func<BattleEvent, bool> condition = null)
        {
            this.battleEventType = battleEventType;
            condition ??= b => true;
            this.condition = condition;
        }
    }
    
    public class BattleEventResponseTrigger : BattleEventTrigger
    {
        public readonly Func<BattleEvent, BattleEventPackage> response;

        public BattleEventResponseTrigger(BattleEventType battleEventType, 
            Func<BattleEvent, BattleEventPackage> response, Func<BattleEvent, bool> condition = null) 
            : base(battleEventType, condition)
        {
            this.response = response;
        }
    }
    
    public class BattleEventActionTrigger : BattleEventTrigger
    {
        public readonly Action<BattleEvent> action;
        
        public BattleEventActionTrigger(BattleEventType battleEventType, Action<BattleEvent> action, 
            Func<BattleEvent, bool> condition = null) : base(battleEventType, condition)
        {
            this.action = action;
        }
        
        public BattleEventActionTrigger(BattleEventType battleEventType,
            Action action, Func<BattleEvent, bool> condition = null) : this(battleEventType,
            b => action.Invoke(), condition)
        {
        }
    }
}