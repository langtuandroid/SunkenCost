using System.Collections;

namespace BattleScreen.ActionsAndResponses
{
    public interface IDamageBattleActionResponder
    {
        public bool GetResponseToDamage(BattleEvent previousBattleEvent);
        public IEnumerator ExecuteResponseToDamage(DamageBattleEvent previousBattleEvent);
    }
}