using System;
using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class ReverseEtching : AboutToMoveActivatedEffect
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = battleEvent.enemyAffectee;
            
            // Set the new goal stick as the opposite direction
            enemy.Mover.Reverse();
            enemy.Mover.AddMovement(GetStatValue(StatType.MovementBoost));
            return new List<BattleEvent>(){CreateEvent(BattleEventType.EnemyMovementModified, DamageSource.Etching, enemy)};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && !enemy.FinishedMoving;
        }
    }
}
