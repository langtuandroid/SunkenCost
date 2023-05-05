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
            var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
            var amountToHeal = enemy.MaxHealth - enemy.Health;

            var responses = new List<BattleEvent> {enemy.Heal(amountToHeal)};
            
            var goldAmount = design.GetStat(StatType.Gold);
            
            var timesMetRequirement = (int)Mathf.Floor((float)amountToHeal / design.GetStat(StatType.IntRequirement));
            var amountOfGoldToGive = design.Level > 1 ? timesMetRequirement * goldAmount : goldAmount;
            
            responses.Add(new BattleEvent(BattleEventType.TryGainedGold) {modifier = amountOfGoldToGive});

            return new DesignResponse(PlankNum, responses);
        }

        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
    }
}