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
    public class BoostEtching : PlankUpdateActivatedEtching
    {
        private readonly List<DamageEtching> _boostedEtchings = new List<DamageEtching>();
        private readonly List<StatModifier> _boostMods = new List<StatModifier>();
        private bool _modsActive = false;
        
        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            var response = new List<DesignResponseTrigger>()
            {
                new DesignResponseTrigger(BattleEventType.EtchingStunned, 
                    b => ClearModsWithoutBoosting(), 
                    b => _modsActive && GetIfThisIsPrimaryResponder(b)),
            };
            
            response.AddRange(base.GetDesignResponseTriggers());
            return response;
        }

        protected override DesignResponse GetPlankUpdateResponse(BattleEvent b)
        {
            return RefreshBoosts();
        }

        protected override List<BattleEventActionTrigger> GetDesignActionTriggers()
        {
            return new List<BattleEventActionTrigger>
            {
                ActionTrigger(BattleEventType.EndedBattle, () => ClearModsWithoutBoosting())
            };
        }

        private DesignResponse ClearModsWithoutBoosting()
        {
            return new DesignResponse(-1, ClearMods(), false);
        }
        
        private DesignResponse RefreshBoosts()
        {
            if (stunned)
                return ClearModsWithoutBoosting();
            
            var responses = new List<BattleEvent>();

            if (_modsActive) 
                responses.AddRange(ClearMods());

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
            
            Debug.Log($"Boost etching ({PlankNum}) boosting: " + String.Join(", ", 
                _boostedEtchings.Select(b => b.Design.Title + "(" + b.PlankNum + ")")));

            // Boost etchings
            foreach (var etching in _boostedEtchings)
            {
                var boostMod = new StatModifier(Design.GetStat(StatType.StatFlatModifier), StatModType.Flat);
                _boostMods.Add(boostMod);
                responses.Add(etching.AddStatModifier(StatType.Damage, boostMod));
            }

            return new DesignResponse(-1, responses, false);
        }

        private List<BattleEvent> ClearMods()
        {
            var notDestroyedEtchings =
                _boostedEtchings.Where(e => e.gameObject != null);
            
            var notDestroyedArray = notDestroyedEtchings as DamageEtching[] ?? notDestroyedEtchings.ToArray();
            var responses = notDestroyedArray.Select(
                (t, i) => t.RemoveStatModifier(StatType.Damage, _boostMods[i])).ToList();

            Debug.Log("Boost etching ("+ PlankNum + ") clearing: " + String.Join(", ", 
                notDestroyedArray.Select(b => b.Design.Title + "(" + b.PlankNum + ")")));
            
            _modsActive = false;
            _boostedEtchings.Clear();
            _boostMods.Clear();
            return responses;
        }
    }
}
