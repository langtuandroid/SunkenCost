using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;

namespace Varnishes
{
    public class EcoFriendlyVarnish : Varnish
    {
        public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                EventResponseTrigger(BattleEventType.StartedBattle, IncreaseUsesPerTurn)
            };
        }

        private BattleEvent IncreaseUsesPerTurn()
        {
            if (Etching.HasStat(StatType.UsesPerTurn))
                return Etching.AddStatModifier(StatType.UsesPerTurn, new StatModifier(1, StatModType.Flat));
            return BattleEvent.None;
        }
    }
}