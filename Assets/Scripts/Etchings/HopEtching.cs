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
        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            enemy.Mover.AddSkip(design.GetStat(StatType.Hop));
            
            var response = new BattleEvent(BattleEventType.EnemyMovementModified)
                    {affectedResponderID = battleEvent.affectedResponderID};
            
            return new DesignResponse(PlankNum, response);;
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            if (enemy.FinishedMoving) return false;
            return enemy.PlankNum - enemy.Mover.LastDirection != PlankNum;
        }
    }
}