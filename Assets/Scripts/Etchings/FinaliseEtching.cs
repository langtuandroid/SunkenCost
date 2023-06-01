using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class FinaliseEtching : DamageEtching
    {
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            var enemiesOnPlanks = EnemySequencer.Current.AllEnemies.Where(e => e.PlankNum > -1).ToArray();

            var response = new List<BattleEvent>();
            var anEnemyWillSurvive = false;

            foreach (var e in enemiesOnPlanks)
            {
                var damageEvent = DamageEnemy(e.ResponderID);
                response.Add(damageEvent);
                
                if (e.Health > damageEvent.modifier)
                    anEnemyWillSurvive = true;
            }
            
            if (anEnemyWillSurvive)
            {
                response.Add(new BattleEvent(BattleEventType.PlayerLifeModified) {modifier = design.GetStat(StatType.PlayerHealthModifier)});
            }

            var allPlanks = new List<int>();
            for (var i = 0; i < Board.Current.PlankCount; i++)
            {
                allPlanks.Add(i);
            }
            return new DesignResponse(allPlanks, response);
        }
    }
}
