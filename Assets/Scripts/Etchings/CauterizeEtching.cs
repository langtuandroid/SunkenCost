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
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum && enemy.stats.Poison > 0;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            var poison = enemy.stats.Poison;

            var newStatMod = new StatModifier
                (-poison * Design.GetStat(StatType.StatMultiplier), StatModType.Flat);
            enemy.AddMaxHealthModifier(newStatMod);

            var response = new BattleEvent(BattleEventType.EnemyMaxHealthModified)
            {
                primaryResponderID = enemy.ResponderID
            };
            
            return new DesignResponse(PlankNum, response);
        }
    }
}