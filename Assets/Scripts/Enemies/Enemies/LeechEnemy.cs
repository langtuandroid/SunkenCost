using System;
using System.Collections.Generic;
using BattleScreen;

namespace Enemies.Enemies
{
    public class LeechEnemy : Enemy, IStartOfTurnAbilityHolder
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

        public override string GetDescription()
        {
            return "At the start of it's turn it leeches health from Steelo";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return (_steelo && !_steelo.IsDestroyed && MaxHealth - Health > 0);
        }

        public List<BattleEvent> GetStartOfTurnAbility()
        {
            var damage = MaxHealth - Health;

            if (_steelo.Health < damage) damage = _steelo.Health;

            var response = new List<BattleEvent>();
            
            response.Add(DamageHandler.DamageEnemy(damage, _steelo, DamageSource.EnemyAbility));
            response.Add(Heal(damage));
            return response;
        }
    }
}