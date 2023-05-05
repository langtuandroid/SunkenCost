using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class BoostEtching : StickMovementActivatedEtching
    {
        private readonly List<DamageEtching> _boostedEtchings = new List<DamageEtching>();
        private StatModifier _boostMod;
        private bool _modsActive = false;
        
        private void ClearMods()
        {
            // Clear stored etchings
            foreach (var etching in _boostedEtchings)
            {
                etching.design.RemoveStatModifier(StatType.Damage, _boostMod);
            }

            _modsActive = false;
            _boostedEtchings.Clear();
        }
        
        protected override bool TestStickMovementActivatedEffect(BattleEvent battleEvent)
        {
            if (stunned)
            {
                if (_modsActive) ClearMods();
                return false;
            }

            if (battleEvent.affectedResponderID == ResponderID) return false;
            return battleEvent.type switch
            {
                BattleEventType.StartNextPlayerTurn => true,
                BattleEventType.EndedBattle => true,
                BattleEventType.PlankDestroyed => true,
                BattleEventType.PlayerMovedPlank => true,
                BattleEventType.PlankCreated => true,
                BattleEventType.DesignModified => true,
                _ => false
            };
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            if (_modsActive) ClearMods();

            // Etching to the left
            if (PlankNum > 1)
            {
                var leftPlank = Board.Current.GetPlank(PlankNum - 1);
                if (leftPlank && leftPlank.Etching && leftPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    _modsActive = true;
                }
            }
            
            // Etching to the right
            if (PlankNum < Board.Current.PlankCount)
            {
                var rightPlank = Board.Current.GetPlank(PlankNum + 1);
                if (rightPlank && rightPlank.Etching && rightPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    _modsActive = true;
                }
            }

            if (_boostedEtchings.Count == 0) 
                return new DesignResponse(-1, new List<BattleEvent>() {BattleEvent.None});

            var responses = new List<BattleEvent>();
            
            _boostMod = new StatModifier(design.GetStat(StatType.DamageFlatModifier), StatModType.Flat);

            foreach (var etching in _boostedEtchings)
            {
                etching.design.AddStatModifier(StatType.Damage, _boostMod);
                responses.Add(new BattleEvent(BattleEventType.DesignModified, etching.ResponderID));
            }

            var planksToColor = new List<int>() {PlankNum};
            planksToColor.AddRange(_boostedEtchings.Select(e => e.PlankNum));
            
            return new DesignResponse(planksToColor, responses);
        }
    }
}
