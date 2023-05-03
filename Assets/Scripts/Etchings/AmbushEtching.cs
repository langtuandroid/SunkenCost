using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;

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
                    UpdateDamage();
                    return false;
                case BattleEventType.EnemyMove
                    when BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID).PlankNum ==
                         PlankNum:
                    _hadEnemyOnPlankThisTurn = true;
                    break;
            }

            return base.GetIfDesignIsRespondingToEvent(battleEvent);
        }

        private void UpdateDamage()
        {
            if (!_hadEnemyOnPlankThisTurn)
            {
                var increase = new StatModifier(design.GetStat(StatType.DamageFlatModifier), StatModType.Flat);
                _statModifiers.Push(increase);
                design.Stats[StatType.Damage].AddModifier(increase);
            }

            _hadEnemyOnPlankThisTurn = false;
        }

        private void OnDestroy()
        {
            foreach (var statModifier in _statModifiers)
            {
                design.Stats[StatType.Damage].RemoveModifier(statModifier);
            }
        }
    }
}