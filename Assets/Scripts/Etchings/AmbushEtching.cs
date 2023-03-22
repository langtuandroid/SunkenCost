namespace Etchings
{
    public class AmbushEtching : MeleeEtching
    {
        private bool _hadEnemyOnPlankThisTurn = false;
        
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
                design.Stats[St.Damage].AddModifier(increase);
            }

            _hadEnemyOnPlankThisTurn = false;
        }
    }
}