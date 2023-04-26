using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class FinaliseEtching : DamageEtching
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var currentMovingEnemy = battleEvent.enemyAffectee;

            var enemiesOnPlanks = EnemySequencer.Current.AllEnemies.Where(e => e.PlankNum != 0).ToArray();

            var response = enemiesOnPlanks.Select(DamageEnemy).ToList();
            
            if (enemiesOnPlanks.Any(e => !e.IsDestroyed))
            {
                response.Add(CreateEvent(BattleEventType.PlayerLifeModified, modifier: -1));
            }

            return response;
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}
