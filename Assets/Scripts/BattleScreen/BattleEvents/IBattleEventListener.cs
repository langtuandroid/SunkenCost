using System.Collections.Generic;

namespace BattleScreen.BattleEvents
{
    public interface IBattleEventListener
    {
        public List<BattleEventResponseTrigger> GetResponseTriggers();
        
        public List<ActionTrigger> GetActionTriggers();
    }
}