using BattleScreen;
using BattleScreen.BattleBoard;
using Enemies;

namespace Etchings
{
    public abstract class MovementActivatedEtching : Etching
    {
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            if (stunned || battleEvent.type != GetEventType()) return false;

            var enemy = battleEvent.enemyAffectee;
            
            // Enemy reached end
            if (enemy.PlankNum > Board.Current.PlankCount) return false;

            return ((design.Limitless || UsesUsedThisTurn < UsesPerTurn) && TestCharMovementActivatedEffect(enemy));
        }

        protected abstract BattleEventType GetEventType();
        
        protected abstract bool TestCharMovementActivatedEffect(Enemy enemy);
        
    }
}