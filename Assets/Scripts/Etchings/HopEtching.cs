using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class HopEtching : AboutToMoveActivatedEffect
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            battleEvent.enemyAffectee.Mover.AddSkip(GetStatValue(StatType.Hop));
            return new List<BattleEvent>(){CreateEvent(BattleEventType.EnemyMovementModified)};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            if (enemy.FinishedMoving) return false;
            return enemy.PlankNum - enemy.Mover.LastDirection != PlankNum;
        }
    }
}