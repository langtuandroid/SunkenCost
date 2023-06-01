using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;

namespace Etchings
{
    public class PoisonEtching : LandedOnPlankActivatedEtching
    {
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            return new DesignResponse(PlankNum, enemy.stats.AddPoison(design.GetStat(StatType.Poison)));
        }
    }
}
