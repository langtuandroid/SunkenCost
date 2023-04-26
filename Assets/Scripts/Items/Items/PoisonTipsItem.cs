using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class PoisonTipsItem : EquippedItem
    {
        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyDamaged && battleEvent.damageSource == DamageSource.Etching;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(battleEvent.enemyAffectee.stats.AddPoison(Amount));
        }
    }
}