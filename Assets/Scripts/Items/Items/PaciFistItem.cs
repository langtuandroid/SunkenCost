using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;

namespace Items.Items
{
    public class PaciFistItem : EquippedItem
    {
        private bool _hasKilledEnemyThisBattle = false;

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.Type == BattleEventType.EnemyKilled ||
                   battleEvent.Type == BattleEventType.EndedBattle;
        }
        
        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            switch (battleEvent.Type)
            {
                case BattleEventType.EnemyKilled:
                    _hasKilledEnemyThisBattle = true;
                    break;
                case BattleEventType.EndedBattle:
                    if (!_hasKilledEnemyThisBattle) return new TryGainGoldBattleEvent(Amount);
                    else break;
                default:
                    throw new UnexpectedBattleEventException(battleEvent);
            }

            return new BattleEvent(BattleEventType.None);
        }
    }
}