using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class PoisonTipsItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyDamaged && battleEvent.damageSource == DamageSource.Etching;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            return battleEvent.enemyAffectee.stats.AddPoison(Amount);
        }
    }
}