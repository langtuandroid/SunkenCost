using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class RestEtching : Etching
    {
        private Stat _healAmountStat;
        
        private void Start()
        {
            _healAmountStat = new Stat(design.GetStat(StatType.HealPlayer));
        }

        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EndedBattle;
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var response = new BattleEvent(BattleEventType.PlayerLifeModified)
                {modifier = _healAmountStat.Value};
            return new DesignResponse(PlankNum, response);
        }
    }
}