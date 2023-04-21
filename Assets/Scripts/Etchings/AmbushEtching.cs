using System.Collections.Generic;
using BattleScreen;
using Designs;

namespace Etchings
{
    public class AmbushEtching : MeleeEtching
    {
        private bool _hadEnemyOnPlankThisTurn = false;

        private readonly Stack<StatModifier> _statModifiers = new Stack<StatModifier>();
        
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            switch (battleEvent.type)
            {
                case BattleEventType.EndedEnemyTurn:
                    UpdateDamage();
                    break;
                case BattleEventType.EnemyMove when battleEvent.enemyAffectee.PlankNum == PlankNum:
                    _hadEnemyOnPlankThisTurn = true;
                    break;
            }

            return base.GetIfRespondingToBattleEvent(battleEvent);
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