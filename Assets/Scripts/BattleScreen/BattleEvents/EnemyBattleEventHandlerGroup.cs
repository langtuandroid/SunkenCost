using System;
using Enemies;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EnemyBattleEventHandlerGroup : BattleEventHandlerGroup
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private EnemySequencer _enemySequencer;
        private Enemy _currentEnemy;

        private void Awake()
        {
            CreateHandler(_enemySpawner);
            CreateHandler(_enemySequencer);
        }

        public override BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            switch (previousBattleEvent.type)
            {
                // Register / Deregister enemies
                case BattleEventType.EnemySpawned:
                    var spawnedEnemy = BattleEventHandler.AllBattleEventRespondersByID[previousBattleEvent
                        .creatorID];
                    if (!HasHandler(spawnedEnemy)) CreateHandler(spawnedEnemy);
                    break;
                case BattleEventType.EnemyKilled:
                    var killedEnemy = BattleEventHandler.AllBattleEventRespondersByID[previousBattleEvent
                        .creatorID];
                    if (HasHandler(killedEnemy)) RemoveHandler(killedEnemy);
                    break;
            }
            
            return base.GetNextResponse(previousBattleEvent);
        }
    }
}