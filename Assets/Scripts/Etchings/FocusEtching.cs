using System.Collections.Generic;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class FocusEtching : ActiveEtching, IDamageMultiplierModifier
    {
        private Stat _boostAmountStat;
        
        protected override void Start()
        {
            _boostAmountStat = new Stat(design.GetStat(StatType.Boost));
            colorWhenActivated = true;
            base.Start();
        }
        
        public int GetDamageModification(int damage, Enemy enemy, DamageSource source, Etching etching = null)
        {
            if ((source != DamageSource.Etching && design.Level < 1) || deactivationTurns > 0) return damage;
            
            if (enemy.Plank == Plank)
            {
                return damage *= _boostAmountStat.Value;
            }

            return damage;
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Plank.GetPlankNum();
        }
    }
}