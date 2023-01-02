using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class StrikeZoneEtching : ActiveEtching, IDamageMultiplierModifier
    {
        private Stat _boostAmountStat;
        
        protected override void Start()
        {
            _boostAmountStat = new Stat(design.GetStat(St.Boost));
            colorWhenActivated = true;
            base.Start();
        }
        
        public int GetDamageModification(int damage, Enemy enemy, DamageSource source, Etching etching = null)
        {
            if ((source != DamageSource.Plank && design.Level < 1) || deactivationTurns > 0) return damage;
            
            if (enemy.Stick == Stick)
            {
                return damage *= _boostAmountStat.Value;
            }

            return damage;
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }
    }
}