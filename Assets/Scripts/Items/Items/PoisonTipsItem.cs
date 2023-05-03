using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Items.Items
{
    public class PoisonTipsItem : EquippedItem
    {
        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyDamaged && battleEvent.source == DamageSource.Etching;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            return new BattleEventPackage(enemy.stats.AddPoison(Amount));
        }
    }
}