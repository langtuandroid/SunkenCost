using System;
using Enemies;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EnemyBattleEventResponderGroup : BattleEventResponderGroup
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private EnemySequencer _enemySequencer;
        private Enemy _currentEnemy;

        private void Awake()
        {
            AddResponder(_enemySpawner);
            AddResponder(_enemySequencer);
        }

        public override BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            switch (previousBattleEvent.type)
            {
                // Register / Deregister enemies
                case BattleEventType.EnemySpawned:
                    var spawnedEnemy = BattleEventResponder.AllBattleEventRespondersByID[previousBattleEvent
                        .primaryResponderID];
                    if (!HasResponder(spawnedEnemy)) AddResponder(spawnedEnemy);
                    break;
                case BattleEventType.EnemyKilled:
                    var killedEnemy = BattleEventResponder.AllBattleEventRespondersByID[previousBattleEvent
                        .primaryResponderID];
                    if (HasResponder(killedEnemy)) RemoveResponder(killedEnemy);
                    break;
            }
            
            return base.GetNextResponse(previousBattleEvent);
        }
    }
}