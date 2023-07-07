using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;
using Stats;
using UnityEngine;

namespace Enemies.Enemies
{
    public class CucungerEnemy : EliteEnemy, IStartOfTurnAbilityHolder
    {
        private int _cooldownCounter = 0;

        public override string GetDescription()
        {
            var turns = stats.GetModifier(EnemyModifierType.Cooldown) - _cooldownCounter;

            var turnText = "";
            switch (turns)
            {
                case 0:
                    turnText = "this turn";
                    break;
                case 1:
                    turnText = "next turn";
                    break;
                default:
                    turnText = "in " + turns + " turns";
                    break;
            }
        
            return "Destroys the furthest plank to it's right " + turnText;
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return true;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            var response = new List<BattleEvent>();

            var abilityCooldown = stats.GetModifier(EnemyModifierType.Cooldown);
        
            var speech = (abilityCooldown - _cooldownCounter).ToString();
            if (speech == "0")
                speech = "X";
            else if (speech == "1")
                speech = "..";

            response.Add(Speak(speech));
        
            if (_cooldownCounter >= abilityCooldown)
            {
                _cooldownCounter = 0;
                
                if (PlankNum != Board.Current.PlankCount - 1)
                    response.AddRange(Board.Current.GetPlank(Board.Current.PlankCount - 1).Destroy(DamageSource.EnemyAbility));
            }
            else
            {
                _cooldownCounter++;
            }

            return new BattleEventPackage(response);
        }
    }
}
