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
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            var amountToHeal = enemy.MaxHealth - enemy.Health;

            var responses = new List<BattleEvent> {enemy.Heal(amountToHeal)};
            
            var goldAmount = Design.GetStat(StatType.Gold);
            
            var timesMetRequirement = (int)Mathf.Floor((float)amountToHeal / Design.GetStat(StatType.IntRequirement));

            // Level 0 can only get maximum of one gold
            if (Design.Level < 2 && timesMetRequirement > 1) timesMetRequirement = 1; 
            
            var amountOfGoldToGive = timesMetRequirement * goldAmount;
            
            responses.Add(new BattleEvent(BattleEventType.TryGainedGold) {modifier = amountOfGoldToGive});

            return new DesignResponse(PlankNum, responses);
        }
    }
}