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

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }

        public bool CanModify(BattleEvent battleEventToModify)
        {
            // Only affects etching damage on level 0
            if (battleEventToModify.damageSource != DamageSource.Etching && design.Level < 1) return false;
            return !stunned && battleEventToModify.enemyAffectee.PlankNum == PlankNum;
        }

        public DamageModification GetDamageMultiplier(BattleEvent battleEventToModify)
        {
            return new DamageModification(this, design.GetStat(StatType.StatMultiplier));
        }
    }
}