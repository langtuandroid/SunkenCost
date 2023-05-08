using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class LoneWolfEtching : MeleeEtching
    {
        private StatModifier _damageMod;
        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var responses = new List<BattleEvent>();

            switch (battleEvent.type)
            {
                case BattleEventType.PlankCreated:
                case BattleEventType.PlankDestroyed:
                case BattleEventType.StartedBattle:
                    responses.Add(UpdateDamage());
                    break;
            }
            
            responses.AddRange(base.GetDesignResponsesToEvent(battleEvent).response);
            return new DesignResponse(PlankNum, responses);
        }

        private BattleEvent UpdateDamage()
        {
            if (_damageMod is not null)
                design.RemoveStatModifier(StatType.Damage, _damageMod);

            var penalty = design.GetStat(StatType.Damage) * (Board.Current.PlankCount - 1);

            _damageMod = new StatModifier(penalty, StatModType.Flat);
            design.AddStatModifier(StatType.Damage, _damageMod);

            return new BattleEvent(BattleEventType.DesignModified);
        }
    }
}
