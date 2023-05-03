using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class HopEtching : AboutToMoveActivatedEffect
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            enemy.Mover.AddSkip(GetStatValue(StatType.Hop));
            return new List<BattleEvent>(){new BattleEvent(BattleEventType.EnemyMovementModified)
                {affectedResponderID = battleEvent.affectedResponderID}};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            if (enemy.FinishedMoving) return false;
            return enemy.PlankNum - enemy.Mover.LastDirection != PlankNum;
        }
    }
}