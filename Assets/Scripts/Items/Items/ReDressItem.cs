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
            return battleEvent.type == BattleEventType.EnemyReachedBoat;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            var responses = new List<BattleEvent>();
            
            var enemies = EnemySequencer.Current.AllEnemies;
            for (var i = enemies.Count - 1 ; i > 0; i--)
            {
                if (!enemies[i] || enemies[i].IsDestroyed) continue;

                var damageEvent = DamageHandler.DamageEnemy(Amount, enemies[i].ResponderID, DamageSource.Item);
                responses.Add(damageEvent);
            }

            return new BattleEventPackage(responses);
        }
    }
}