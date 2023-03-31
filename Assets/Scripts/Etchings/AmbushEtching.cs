using System.Collections.Generic;

namespace Etchings
{
    public class AmbushEtching : MeleeEtching
    {
        private bool _hadEnemyOnPlankThisTurn = false;

        private readonly Stack<StatModifier> _statModifiers = new Stack<StatModifier>();
        
        protected override void Start()
        {
            BattleEvents.Current.OnBeginPlayerTurn += UpdateDamage;
            base.Start();
        }

        protected override bool TestCharMovementActivatedEffect()
        {
            var success = base.TestCharMovementActivatedEffect();

            if (success)
            {
                _hadEnemyOnPlankThisTurn = true;
            }

            return success;
        }

        private void UpdateDamage()
        {
            if (!_hadEnemyOnPlankThisTurn)
            {
                var increase = new StatModifier(design.GetStat(St.DamageFlatModifier), StatModType.Flat);
                _statModifiers.Push(increase);
                design.Stats[St.Damage].AddModifier(increase);
            }

            _hadEnemyOnPlankThisTurn = false;
        }

        private void OnDestroy()
        {
            foreach (var statModifier in _statModifiers)
            {
                design.Stats[St.Damage].RemoveModifier(statModifier);
            }
        }
    }
}