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
        
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            switch (battleEvent.type)
            {
                case BattleEventType.StartNextPlayerTurn:
                    var hadEnemyOnPlankThisTurn = _hadEnemyOnPlankThisTurn;
                    _hadEnemyOnPlankThisTurn = false;
                    return (!hadEnemyOnPlankThisTurn && Battle.Current.Turn > 1);
                case BattleEventType.EnemyMove
                    when BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID).PlankNum ==
                         PlankNum:
                    _hadEnemyOnPlankThisTurn = true;
                    break;
            }

            return base.GetIfDesignIsRespondingToEvent(battleEvent);
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.StartNextPlayerTurn)
            {
                _hadEnemyOnPlankThisTurn = false; 
                var newStatMod = new StatModifier(design.GetStat(StatType.DamageFlatModifier), StatModType.Flat);
                _statModifiers.Push(newStatMod); 
                design.AddStatModifier(StatType.Damage, newStatMod);
                var response = new BattleEvent(BattleEventType.DesignModified) {affectedResponderID = ResponderID};
                return new DesignResponse(-1, response);
            }
            
            Debug.Log("responding to " + battleEvent.type);
            return base.GetDesignResponsesToEvent(battleEvent);
        }

        private void OnDestroy()
        {
            foreach (var statMod in _statModifiers)
            {
                design.AddStatModifier(StatType.Damage, statMod);
            }
        }
    }
}