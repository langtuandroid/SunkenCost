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

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            
            return new List<BattleEvent>() 
                {enemy.stats.AddPoison(_poisonStat.Value)};
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}
