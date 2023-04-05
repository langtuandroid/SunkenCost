using System.Collections;

namespace BattleScreen
{
    public interface IBattleActionResponder
    {
        public bool GetResponseToBattleEvent(BattleEvent previousBattleEvent);
        public IEnumerator ExecuteResponseToAction(BattleEvent battleEvent);
    }
}