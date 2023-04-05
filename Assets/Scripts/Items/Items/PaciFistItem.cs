using System;
using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class PaciFistItem : BattleEventResponderItem
    {
        private bool _hasKilledEnemyThisBattle = false;

        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.EnemyKilled ||
                   previousBattleEvent.battleEventType == BattleEventType.EndBattle;
        }

        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            if (battleEvent.battleEventType == BattleEventType.EnemyKilled)
            {
                _hasKilledEnemyThisBattle = true;
            }
            else if (battleEvent.battleEventType == BattleEventType.EndBattle)
            {
                // TODO: Make Alter Gold a coroutine
                if (!_hasKilledEnemyThisBattle)
                {
                    RunProgress.PlayerStats.AlterGold(Amount);
                }
                yield break;
            }
            else
            {
                throw new Exception("Pacifist should not be responding to " + battleEvent.battleEventType);
            }
        }
    }
}