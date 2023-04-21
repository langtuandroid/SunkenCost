using System.Collections.Generic;
using BattleScreen;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class ResearchEtching : LandedOnPlankActivatedEtching
    {
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = battleEvent.enemyAffectee;
            var amountToHeal = enemy.MaxHealth - enemy.Health;

            var responses = new List<BattleEvent>();
            
            responses.Add(enemy.Heal(amountToHeal));

            var timesMetRequirement = (int)Mathf.Floor(((float)amountToHeal / GetStatValue(StatType.IntRequirement)));
            var amountOfGoldToGive = timesMetRequirement * GetStatValue(StatType.Gold);
            
            responses.Add(CreateEvent(BattleEventType.TryGainedGold, modifier: amountOfGoldToGive));

            return responses;
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}