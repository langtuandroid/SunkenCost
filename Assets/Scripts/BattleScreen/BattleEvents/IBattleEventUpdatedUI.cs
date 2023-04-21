using System.Collections.Generic;

namespace BattleScreen.BattleEvents
{
    public interface IBattleEventUpdatedUI
    {
        public bool GetIfUpdating(BattleEvent battleEvent);
        public void SaveCurrentState();
        public void LoadNextState();
    }
}