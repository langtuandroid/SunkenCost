using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class ResearchEtching : LandedOnPlankActivatedEtching
    {
        protected override DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            var enemy = battleEvent.Enemy;
            var amountToHeal = enemy.MaxHealth - enemy.Health;

            var responses = new List<BattleEvent> {enemy.Heal(amountToHeal)};
            
            var goldAmount = design.GetStat(StatType.Gold);
            
            var timesMetRequirement = (int)Mathf.Floor((float)amountToHeal / design.GetStat(StatType.IntRequirement));

            // Level 0 can only get maximum of one gold
            if (design.Level < 2 && timesMetRequirement > 1) timesMetRequirement = 1; 
            
            var amountOfGoldToGive = timesMetRequirement * goldAmount;
            
            responses.Add(new BattleEvent(BattleEventType.TryGainedGold) {modifier = amountOfGoldToGive});

            return new DesignResponse(PlankNum, responses);
        }

        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}