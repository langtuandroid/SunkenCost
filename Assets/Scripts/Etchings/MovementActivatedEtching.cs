using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies;

namespace Etchings
{
    public abstract class MovementActivatedEtching : Etching
    {
        protected override List<DesignResponseTrigger> GetDesignResponseTriggers()
        {
            return new List<DesignResponseTrigger>
            {
                new DesignResponseTrigger(GetEventType(), 
                    b => GetResponseToMovement(b.Enemy), 
                    b => MovementActivationCondition(b.Enemy))
            };
        }

        protected abstract BattleEventType GetEventType();
        
        protected abstract bool GetIfRespondingToEnemyMovement(Enemy enemy);

        protected abstract DesignResponse GetResponseToMovement(Enemy enemy);
        
        private bool MovementActivationCondition(Enemy enemy)
        {
            if (stunned || enemy.IsDestroyed) return false;
            
            // Enemy reached end
            if (enemy.PlankNum > Board.Current.PlankCount) return false;

            return ((design.Limitless || UsesUsedThisTurn < UsesPerTurn) && GetIfRespondingToEnemyMovement(enemy));
        }
    }
}