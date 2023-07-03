using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using JetBrains.Annotations;
using UnityEngine;

namespace Etchings
{
    public class AmbushEtching : MeleeEtching
    {
        private bool _hadEnemyOnPlankThisTurn = false;
        private int _amountOfTurns = 0;

        private StatModifier _currentStatMod;
        
        private void OnDestroy()
        {
            ClearStatMod();
        }

        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            var responses = new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(BattleEventType.StartedNextPlayerTurn, 
                    b => UpdateDamage(), 
                    b => !_hadEnemyOnPlankThisTurn && Battle.Current.Turn > 1),
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
                ActionTrigger(BattleEventType.EndedBattle, ClearStatMod)
            };
        }

        private DesignResponse UpdateDamage()
        {
            _amountOfTurns++;
            _hadEnemyOnPlankThisTurn = false;
            
            Debug.Log($"Damage pre: {Design.GetStat(StatType.Damage)}");
            
            var statBase = Design.GetStatBase(StatType.Damage);
            var currentBase = _currentStatMod is not null ? (int) _currentStatMod.Value + statBase : statBase;

            ClearStatMod();
            
            _currentStatMod = new StatModifier(Design.GetStat(StatType.StatMultiplier) * currentBase - statBase, StatModType.Flat);
            Debug.Log(_currentStatMod.Value);
            
            Design.AddStatModifier(StatType.Damage, _currentStatMod);
            Debug.Log($"Damage post: {Design.GetStat(StatType.Damage)}");
            var response = new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
            return new DesignResponse(-1, response);
        }

        private void ClearStatMod()
        {
            if (_currentStatMod is not null)
            {
                Design.RemoveStatModifier(StatType.Damage, _currentStatMod);
            }
        }
    }
}