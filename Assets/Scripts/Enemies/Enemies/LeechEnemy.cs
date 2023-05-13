using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Enemies.Enemies
{
    public class LeechEnemy : EliteEnemy, IStartOfTurnAbilityHolder
    {
        private SteeloEnemy _steelo;

        private void Start()
        {
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

        public BattleEventPackage GetStartOfTurnAbility()
        {
            var damage = MaxHealth - Health;

            if (_steelo.Health < damage) damage = _steelo.Health;
            
            return new BattleEventPackage(DamageHandler.DamageEnemy(damage, _steelo.ResponderID, DamageSource.EnemyAbility),
                Heal(damage));
        }
    }
}