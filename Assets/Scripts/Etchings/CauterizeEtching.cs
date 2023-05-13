using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
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

        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = battleEvent.Enemy;
            var poison = enemy.stats.Poison;

            var newStatMod = new StatModifier
                (-poison * design.GetStat(StatType.StatMultiplier), StatModType.Flat);
            enemy.AddMaxHealthModifier(newStatMod);

            var response = new BattleEvent(BattleEventType.EnemyMaxHealthModified)
            {
                primaryResponderID = battleEvent.primaryResponderID
            };
            
            return new DesignResponse(PlankNum, response);
        }
    }
}