using System;

namespace Enemies.Enemies
{
    public class LeechEnemy : Enemy
    {
        private SteeloEnemy _steelo;
        
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Leech";
            Mover.AddMove(2);
            SetInitialHealth(40);
            Gold = 3;

            _steelo = FindObjectOfType<SteeloEnemy>();
            if (!_steelo)
            {
                throw new Exception("No Steelo found!");
            }
        }

        protected override bool HasStartOfTurnAbility()
        {
            return (_steelo && !_steelo.IsDestroyed);
        }

        protected override void StartOfTurnAbility()
        {
            var damage = MaxHealth.Value - Health;
            if (damage <= 0) return;
            
            if (_steelo.Health < damage) damage = _steelo.Health;
            
            DamageHandler.DamageEnemy(damage, _steelo, DamageSource.EnemyAbility);
            Heal(damage);
        }

        public override string GetDescription()
        {
            return "At the start of it's turn it leeches health from Steelo";
        }
    }
}