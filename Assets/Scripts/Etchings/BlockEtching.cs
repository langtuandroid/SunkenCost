using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class BlockEtching : AboutToMoveActivatedEffect
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            return new List<BattleEvent>() {battleEvent.enemyAffectee.Block(design.GetStat(StatType.Block))};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return (enemy.PlankNum == PlankNum && !enemy.FinishedMoving);
        }
    }
}