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
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            return false;
        }

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }

        public bool CanModify(EnemyDamage enemyDamage)
        {
            // Only affects etching damage on level 0
            if (enemyDamage.source != DamageSource.Etching && design.Level < 1) return false;
            return !stunned && enemyDamage.affectedEnemy.PlankNum == PlankNum;
        }

        public DamageModification GetDamageMultiplier(EnemyDamage enemyDamage)
        {
            return new DamageModification(this, design.GetStat(StatType.StatMultiplier));
        }
    }
}