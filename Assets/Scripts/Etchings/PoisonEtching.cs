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
        private Stat _poisonStat;

        private void Start()
        {
            _poisonStat = new Stat(design.GetStat(StatType.Poison));
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            return new DesignResponse(PlankNum, battleEvent.Enemy.stats.AddPoison(_poisonStat.Value));
        }

        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}
