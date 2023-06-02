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
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return (enemy.PlankNum == PlankNum && !enemy.FinishedMoving);
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            return new DesignResponse(PlankNum, enemy.Block(Design.GetStat(StatType.Block)));
        }
    }
}