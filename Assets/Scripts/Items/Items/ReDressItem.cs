using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;

namespace Items.Items
{
    public class ReDressItem : EquippedItem
    {
        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyAttackedBoat && !battleEvent.Enemy.IsDestroyed;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(DamageHandler.DamageEnemy(Amount, battleEvent.primaryResponderID, DamageSource.Item));
        }
    }
}