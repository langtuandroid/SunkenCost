using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class CauterizeEtching : LandedOnPlankActivatedEtching
    {
        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && enemy.stats.Poison > 0;
        }

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = battleEvent.enemyAffectee;
            var poison = enemy.stats.Poison;

            var newStatMod = new StatModifier
                (-poison * GetStatValue(StatType.StatMultiplier), StatModType.Flat);
            enemy.AddMaxHealthModifier(newStatMod);
            return new List<BattleEvent>(){CreateEvent(BattleEventType.EnemyMaxHealthModified)};
        }
    }
}