using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class RestEtching : Etching
    {
        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            return new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(BattleEventType.EndedBattle, b => HealPlayer())
            };
        }
        

        private DesignResponse HealPlayer()
        {
            var response = new BattleEvent(BattleEventType.PlayerLifeModified)
                {modifier = Design.GetStat(StatType.PlayerHealthModifier)};
            return new DesignResponse(PlankNum, response);
        }
    }
}