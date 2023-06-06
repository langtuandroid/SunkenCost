using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class LoneWolfEtching : MeleeEtching
    {
        private StatModifier _damageMod;

        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            var response = new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(BattleEventType.StartedBattle, b => UpdateDamage()), 
                new DesignResponseTrigger(BattleEventType.PlankCreated, b => UpdateDamage()), 
                new DesignResponseTrigger(BattleEventType.PlankDestroyed, b => UpdateDamage()), 
            };
            
            response.AddRange(base.GetDesignResponseTriggers());

            return response;
        }

        protected override List<BattleEventActionTrigger> GetDesignActionTriggers()
        {
            return new List<BattleEventActionTrigger>
            {
                ActionTrigger(BattleEventType.EndedBattle, RemoveMod)
            };
        }
        
        private DesignResponse UpdateDamage()
        {
            var penalty = Design.GetStat(StatType.StatFlatModifier) * (Board.Current.PlankCount - 1);

            _damageMod = new StatModifier(penalty, StatModType.Flat);
            Design.AddStatModifier(StatType.Damage, _damageMod);

            var designModificationEvent = new BattleEvent(BattleEventType.DesignModified)
            {
                primaryResponderID = ResponderID, showResponse = false
            };
            
            return new DesignResponse(-1, designModificationEvent, showResponse: false);
        }

        private void RemoveMod()
        {
            if (_damageMod is not null)
                Design.RemoveStatModifier(StatType.Damage, _damageMod);
        }
    }
}
