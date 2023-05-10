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
        private readonly List<StatModifier> _boostMods = new List<StatModifier>();
        private bool _modsActive = false;

        protected override bool TestStickMovementActivatedEffect(BattleEvent battleEvent)
        {
            if (stunned && _modsActive) return true;
            
            switch (battleEvent.type)
            {
                case BattleEventType.StartNextPlayerTurn:
                case BattleEventType.EndedBattle:
                case BattleEventType.PlankDestroyed:
                case BattleEventType.PlankMoved:
                case BattleEventType.EtchingMoved:
                case BattleEventType.PlankCreated:
                    return true;
                case BattleEventType.DesignModified:
                    return battleEvent.affectedResponderID == ResponderID;
                default:
                    return false;
            }
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var responses = new List<BattleEvent>();

            if (_modsActive) 
                responses.AddRange(ClearMods());
            if (stunned || battleEvent.type == BattleEventType.EndedBattle) 
                return new DesignResponse(-1, responses, false);

            // Etching to the left
            if (PlankNum > 0)
            {
                var leftPlank = Board.Current.GetPlank(PlankNum - 1);
                if (leftPlank && leftPlank.Etching && leftPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    _modsActive = true;
                }
            }
            
            // Etching to the right
            if (PlankNum < Board.Current.PlankCount - 1)
            {
                var rightPlank = Board.Current.GetPlank(PlankNum + 1);
                if (rightPlank && rightPlank.Etching && rightPlank.Etching is DamageEtching damageEtching)
                {
                    _boostedEtchings.Add(damageEtching);
                    _modsActive = true;
                }
            }
            
            // Didn't find any
            if (_boostedEtchings.Count == 0) 
                return new DesignResponse(-1, responses, false);

            
            // Boost etchings
            foreach (var etching in _boostedEtchings)
            {
                var boostMod = new StatModifier(design.GetStat(StatType.StatFlatModifier), StatModType.Flat);
                _boostMods.Add(boostMod);
                responses.Add(etching.AddStatModifier(StatType.Damage, boostMod));
            }

            return new DesignResponse(-1, responses, false);

        }
        
        private List<BattleEvent> ClearMods()
        {
            var responses = _boostedEtchings.Select(
                (t, i) => t.RemoveStatModifier(StatType.Damage, _boostMods[i])).ToList();

            _modsActive = false;
            _boostedEtchings.Clear();
            _boostMods.Clear();
            return responses;
        }
    }
}
