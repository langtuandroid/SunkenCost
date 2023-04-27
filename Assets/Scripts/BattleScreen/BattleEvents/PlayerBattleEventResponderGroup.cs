using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class PlayerBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void Start()
        {
            AddResponder(Player.Current);
        }
    }
}