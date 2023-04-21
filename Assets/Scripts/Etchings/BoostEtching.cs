using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class BoostEtching : StickMovementActivatedEtching
    {
        private readonly List<DamageEtching> _boostedEtchings = new List<DamageEtching>();
        private Stat _boostAmountStat;
        private int _boostAmount => _boostAmountStat.Value;
        private bool modsActive = false;

        protected void Start()
        {
            _boostAmountStat = new Stat(design.GetStat(StatType.Boost));
        }

        private void ClearMods()
        {
            // Clear stored etchings
            foreach (var etching in _boostedEtchings)
            {
                etching.ModifyStat(StatType.Damage, -_boostAmount);
            }

            modsActive = false;
            _boostedEtchings.Clear();
        }
        
        protected override bool TestStickMovementActivatedEffect(BattleEvent battleEvent)
        {
            if (deactivated)
            {
                if (modsActive) ClearMods();
                return false;
            }
            
            return battleEvent.type switch
            {
                BattleEventType.EndedEnemyTurn => true,
                BattleEventType.EndedBattle => true,
                BattleEventType.PlankDestroyed => true,
                BattleEventType.PlayerMovedPlank => true,
                BattleEventType.PlankCreated => true,
                BattleEventType.DesignModified => true,
                _ => false
            };
        }

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            if (modsActive) ClearMods();

            // Etching to the left
            if (PlankNum > 1)
            {
                var leftPlank = PlankMap.Current.GetPlank(PlankNum - 1);
                if (leftPlank && leftPlank.Etching && leftPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    modsActive = true;
                }
            }
            
            // Etching to the right
            if (PlankNum < PlankMap.Current.PlankCount)
            {
                var rightPlank = PlankMap.Current.GetPlank(PlankNum + 1);
                if (rightPlank && rightPlank.Etching && rightPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    modsActive = true;
                }
            }

            if (_boostedEtchings.Count == 0) return new List<BattleEvent>() {BattleEvent.None};

            var events = new List<BattleEvent>();

            foreach (var etching in _boostedEtchings)
            {
                etching.ModifyStat(StatType.Damage, +_boostAmount);
                events.Add(CreateEvent(BattleEventType.DesignModified));
            }

            return events;
        }
    }
}
