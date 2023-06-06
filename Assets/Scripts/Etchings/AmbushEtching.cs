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
        
        protected override List<BattleEventActionTrigger> GetDesignActionTriggers()
        {
            return new List<BattleEventActionTrigger>
            {
                ActionTrigger(BattleEventType.StartedNextPlayerTurn, () => _hadEnemyOnPlankThisTurn = false),
                ActionTrigger(BattleEventType.EnemyMoved, () => _hadEnemyOnPlankThisTurn = true, 
                    b => b.Enemy.PlankNum == PlankNum),
                ActionTrigger(BattleEventType.EndedBattle, RemoveAllStatMods)
            };
        }

        private DesignResponse UpdateDamage()
        {
            _hadEnemyOnPlankThisTurn = false; 
            var newStatMod = new StatModifier(Design.GetStat(StatType.StatFlatModifier), StatModType.Flat);
            _statModifiers.Push(newStatMod); 
            Design.AddStatModifier(StatType.Damage, newStatMod);
            var response = new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
            return new DesignResponse(-1, response);
        }

        private void RemoveAllStatMods()
        {
            foreach (var statMod in _statModifiers)
            {
                Design.RemoveStatModifier(StatType.Damage, statMod);
            }
        }
    }
}