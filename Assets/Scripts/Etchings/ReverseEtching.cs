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
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            
            // Set the new goal stick as the opposite direction
            enemy.Mover.Reverse();
            enemy.Mover.AddMovement(GetStatValue(StatType.MovementBoost));
            return new List<BattleEvent>(){new BattleEvent(BattleEventType.EnemyMovementModified) 
                {source = DamageSource.Etching, affectedResponderID = battleEvent.affectedResponderID}};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && !enemy.FinishedMoving;
        }
    }
}
