using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleScreen
{
    public class EnemyController : MonoBehaviour
    {
        private List<Enemy> _enemies = new List<Enemy>();
        private Queue<Enemy> _enemyCurrentTurnMoveQueue = new Queue<Enemy>();

        public int NumberOfEnemies => _enemies.Count;

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            _enemyCurrentTurnMoveQueue.Enqueue(enemy);
        }

        public Enemy GetRandomEnemy()
        {
            return _enemies.GetRandom();
        }

        public IEnumerator ExecuteEnemyTurn()
        {
            RefreshEnemies();
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);

            while (_enemyCurrentTurnMoveQueue.Count > 0)
            {
                var enemy = _enemyCurrentTurnMoveQueue.Dequeue();
                
                if (!EnemyIsAlive(enemy)) continue;

                // Start of turn - wait for responses
                var enemyStartTurnAction = new EnemyBattleEvent(BattleEventType.EnemyStartTurn, enemy);
                yield return StartCoroutine
                    (BattleState.current.BattleEvents.ExecuteTurnActionResponses(enemyStartTurnAction));
                
                // Wait for any effects on the enemy - poison, start of turn ability etc
                yield return StartCoroutine(enemy.BeginMyTurn());
                
                // Move sequence
                while (EnemyIsAlive(enemy) && !enemy.FinishedMoving)
                {
                    
                    // Move
                    yield return StartCoroutine(enemy.ExecuteMoveStep());
                    
                    // Wait for responses
                    var enemyMovedStepTurnAction = new EnemyBattleEvent(BattleEventType.EnemyMove, enemy);
                    yield return StartCoroutine
                        (BattleState.current.BattleEvents.ExecuteTurnActionResponses(enemyMovedStepTurnAction));
                }
                
                // TODO: End of turn processing
                yield return StartCoroutine(enemy.EndMyTurn());
            }
        }

        private void RefreshEnemies()
        {
            _enemies = _enemies.Where(EnemyIsAlive).ToList();
        }

        private static bool EnemyIsAlive(Enemy enemy)
        {
            return enemy && !enemy.IsDestroyed;
        }
    }
}