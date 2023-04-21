using Enemies;

namespace BattleScreen.BattleEvents
{
    public class EnemyBattleEventResponderGroup : BattleEventResponderGroup
    {
        public void EnemySpawned(Enemy enemy)
        {
            AddResponder(enemy);
        }

        public void EnemyKilled(Enemy enemy)
        {
            RemoveResponder(enemy);
        }

        public override BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            if (previousBattleEvent.type == BattleEventType.EnemyKilled)
            {
                RemoveResponder(previousBattleEvent.enemyAffectee);
            }
            
            return base.GetEventResponders(previousBattleEvent);
        }
    }
}