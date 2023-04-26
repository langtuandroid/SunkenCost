using System.Collections.Generic;

namespace BattleScreen.BattleEvents
{
    public class BattleEventResponderTracker
    {
        private Dictionary<BattleEvent, int> _iterationTracker = new Dictionary<BattleEvent, int>();
        
        public int GetIndex(BattleEvent battleEvent)
        {
            if (_iterationTracker.TryGetValue(battleEvent, out var iterations))
            {
                return iterations;
            }
            
            _iterationTracker.Add(battleEvent, 0);
            return 0;
        }

        public void SetIndex(BattleEvent battleEvent, int index)
        {
            _iterationTracker[battleEvent] = index;
        }
    }
}