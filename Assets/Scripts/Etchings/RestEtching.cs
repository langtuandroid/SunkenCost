using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class RestEtching : Etching
    {
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EndedBattle;
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var response = new BattleEvent(BattleEventType.PlayerLifeModified)
                {modifier = design.GetStat(StatType.PlayerHealthModifier)};
            return new DesignResponse(PlankNum, response);
        }
    }
}