using System.Collections.Generic;
using BattleScreen;
using Damage;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class FocusEtching : Etching, IDamageMultiplierModifier
    {

        public bool CanModify(EnemyDamage enemyDamage)
        {
            // Only affects etching damage on level 0
            if (enemyDamage.source != DamageSource.Etching && Design.Level < 1) return false;
            return !stunned && enemyDamage.affectedEnemy.PlankNum == PlankNum;
        }

        public DamageModification GetDamageMultiplier(EnemyDamage enemyDamage)
        {
            return new DamageModification(this, Design.GetStat(StatType.StatMultiplier));
        }

        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            return new List<DesignResponseTrigger>();
        }
    }
}