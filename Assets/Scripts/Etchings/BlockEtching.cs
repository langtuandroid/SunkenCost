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
            return new DesignResponse(PlankNum, battleEvent.Enemy.Block(design.GetStat(StatType.Block)));
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return (enemy.PlankNum == PlankNum && !enemy.FinishedMoving);
        }
    }
}