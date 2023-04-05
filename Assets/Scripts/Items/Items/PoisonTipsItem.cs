using System;
using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class PoisonTipsItem : BattleEventResponderItem
    {
        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent is EtchingDamageBattleEvent;
        }

        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            if (battleEvent is EtchingDamageBattleEvent etchingDamageBattleEvent)
            {
                etchingDamageBattleEvent.enemy.stats.AddPoison(Amount);
            }
            else
            {
                throw new Exception("Poison Tips shouldn't be responding!");
            }
            
            yield break;
        }
    }
}