using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class BlockEtching : AboutToMoveActivatedEffect
    {
        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var response = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID)
                    .Block(design.GetStat(StatType.Block));
            
            return new DesignResponse(PlankNum, response);
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return (enemy.PlankNum == PlankNum && !enemy.FinishedMoving);
        }
    }
}