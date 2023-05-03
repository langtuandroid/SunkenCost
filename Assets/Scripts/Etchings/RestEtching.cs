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

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            return new List<BattleEvent>(){new BattleEvent(BattleEventType.PlayerLifeModified) 
                {modifier = _healAmountStat.Value}};
        }
    }
}