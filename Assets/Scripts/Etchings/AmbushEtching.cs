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

        private readonly List<StatModifier> _statModifiers = new List<StatModifier>();
        
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
                ActionTrigger(BattleEventType.EndedBattle, RemoveAllStatMods)
            };
        }

        private DesignResponse UpdateDamage()
        {
            _hadEnemyOnPlankThisTurn = false; 
            var newStatMod = new StatModifier(Design.GetStat(StatType.StatMultiplier), StatModType.Multiply);
            _statModifiers.Add(newStatMod); 
            Debug.Log($"Damage pre: {Design.GetStat(StatType.Damage)}");
            Design.AddStatModifier(StatType.Damage, newStatMod);
            Debug.Log($"Damage post: {Design.GetStat(StatType.Damage)}");
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