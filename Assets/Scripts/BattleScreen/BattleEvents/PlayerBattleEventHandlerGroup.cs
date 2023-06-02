using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class PlayerBattleEventHandlerGroup : BattleEventHandlerGroup
    {
        private void Start()
        {
            CreateHandler(Player.Current);
        }
    }
}