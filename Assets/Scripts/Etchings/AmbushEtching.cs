using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class AmbushEtching : MeleeEtching
    {
        private bool _hadEnemyOnPlankThisTurn = false;

        private readonly Stack<StatModifier> _statModifiers = new Stack<StatModifier>();
        
        private void OnDestroy()
        {
            RemoveAllStatMods();
        }

        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            var responses = new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(BattleEventType.StartedNextPlayerTurn, 
                    b => UpdateDamage(), 
                    b => !_hadEnemyOnPlankThisTurn),
            };

            responses.AddRange(base.GetDesignResponseTriggers());
            
            return responses;
        }
        
        protected override List<ActionTrigger> GetDesignActionTriggers()
        {
            return new List<ActionTrigger>
            {
                AddActionTrigger(BattleEventType.StartedNextPlayerTurn, () => _hadEnemyOnPlankThisTurn = false),
                AddActionTrigger(BattleEventType.EnemyMoved, () => _hadEnemyOnPlankThisTurn = true, 
                    b => b.Enemy.PlankNum == PlankNum),
                AddActionTrigger(BattleEventType.EndedBattle, RemoveAllStatMods)
            };
        }

        private DesignResponse UpdateDamage()
        {
            _hadEnemyOnPlankThisTurn = false; 
            var newStatMod = new StatModifier(design.GetStat(StatType.StatFlatModifier), StatModType.Flat);
            _statModifiers.Push(newStatMod); 
            design.AddStatModifier(StatType.Damage, newStatMod);
            var response = new BattleEvent(BattleEventType.DesignModified) {creatorID = ResponderID};
            return new DesignResponse(-1, response);
        }

        private void RemoveAllStatMods()
        {
            foreach (var statMod in _statModifiers)
            {
                design.RemoveStatModifier(StatType.Damage, statMod);
            }
        }
    }
}