using BattleScreen;

namespace Etchings
{
    public abstract class AboutToMoveActivatedEffect : MovementActivatedEtching
    {
        protected override BattleEventType GetEventType()
        {
            return BattleEventType.EnemyStartMove;
        }
    }
}
