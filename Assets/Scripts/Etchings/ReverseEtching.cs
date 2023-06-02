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
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && !enemy.FinishedMoving;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            // Set the new goal stick as the opposite direction
            enemy.Mover.Reverse();
            enemy.Mover.AddMovement(design.GetStat(StatType.MovementBoost));
            var response = new BattleEvent(BattleEventType.EnemyMovementModified) 
                {source = DamageSource.Etching, creatorID = enemy.ResponderID};
            
            return new DesignResponse(PlankNum, response);
        }
    }
}
