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
        private Stat _baseDamage;
        private Stat _baseDamageModifier;
        private StatModifier _damageMod;

        private void Start()
        {
            _baseDamage = new Stat(design.GetStat(StatType.Damage));
            _baseDamageModifier = new Stat(design.GetStat(StatType.DamageFlatModifier));
        }

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
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
            
            responses.AddRange(base.GetDesignResponsesToEvent(battleEvent));
            return responses;
        }

        private BattleEvent UpdateDamage()
        {
            if (_damageMod is not null)
                _baseDamage.RemoveModifier(_damageMod);

            var penalty = _baseDamageModifier.Value * (Board.Current.PlankCount - 2);

            _damageMod = new StatModifier(penalty, StatModType.Flat);
            _baseDamage.AddModifier(_damageMod);

            return new BattleEvent(BattleEventType.DesignModified);
        }
    }
}
