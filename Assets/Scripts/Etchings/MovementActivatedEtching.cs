using BattleScreen;
using Enemies;

namespace Etchings
{
    public abstract class MovementActivatedEtching : Etching
    {
        protected override bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent)
        {
            if (deactivated || battleEvent.type != GetEventType()) return false;

            var enemy = battleEvent.enemyAffectee;
            
            // Enemy reached end
            if (enemy.PlankNum >= PlankMap.Current.PlankCount + 1) return false;

            return ((design.Limitless || UsesUsedThisTurn < UsesPerTurn) && TestCharMovementActivatedEffect(enemy));
        }

        protected abstract BattleEventType GetEventType();
        
        protected abstract bool TestCharMovementActivatedEffect(Enemy enemy);
        
    }
}