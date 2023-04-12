using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;

namespace Items.Items
{
    public class PoisonTipsItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent is EtchingEnemyDamageBattleEvent;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            if (battleEvent is EtchingEnemyDamageBattleEvent etchingDamageBattleEvent)
            {
                return etchingDamageBattleEvent.enemy.stats.AddPoison(Amount);
            }
            else
            {
                throw new UnexpectedBattleEventException(battleEvent);
            }
        }
    }
}