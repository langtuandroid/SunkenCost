using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class ReDressItem : EquippedItem
    {
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EnemyReachedBoat;
        }

        public override List<BattleEvent> GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            var responses = new List<BattleEvent>();
            
            var enemies = EnemyController.Current.AllEnemies;
            for (var i = enemies.Count - 1 ; i > 0; i--)
            {
                if (!enemies[i] || enemies[i].IsDestroyed) continue;

                var damageEvent = DamageHandler.DamageEnemy(Amount, enemies[i], DamageSource.Item);
                responses.AddRange(BattleEventsManager.Current.GetEventAndResponsesList(damageEvent));
            }

            return responses;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}