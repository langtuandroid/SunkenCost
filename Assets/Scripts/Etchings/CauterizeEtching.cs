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

        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            var poison = enemy.stats.Poison;

            var newStatMod = new StatModifier
                (-poison * GetStatValue(StatType.StatMultiplier), StatModType.Flat);
            enemy.AddMaxHealthModifier(newStatMod);
            return new List<BattleEvent>(){new BattleEvent(BattleEventType.EnemyMaxHealthModified) 
                {affectedResponderID = battleEvent.affectedResponderID}};
        }
    }
}