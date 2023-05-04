using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class ReverseEtching : AboutToMoveActivatedEffect
    {
        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            
            // Set the new goal stick as the opposite direction
            enemy.Mover.Reverse();
            enemy.Mover.AddMovement(GetStatValue(StatType.MovementBoost));
            var response = new BattleEvent(BattleEventType.EnemyMovementModified) 
                {source = DamageSource.Etching, affectedResponderID = battleEvent.affectedResponderID};
            
            return new DesignResponse(PlankNum, response);
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && !enemy.FinishedMoving;
        }
    }
}
