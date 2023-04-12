using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleScreen
{
    public class EnemyController : MonoBehaviour
    {
        public static EnemyController Current;
        
        private List<Enemy> _enemies = new List<Enemy>();
        private Queue<Enemy> _enemyCurrentTurnMoveQueue = new Queue<Enemy>();

        public int NumberOfEnemies => _enemies.Count;
        public ReadOnlyCollection<Enemy> AllEnemies => new ReadOnlyCollection<Enemy>(_enemies);

        private void Awake()
        {
            if (Current) Destroy(Current.gameObject);
            Current = this;
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            _enemyCurrentTurnMoveQueue.Enqueue(enemy);
        }

        public Enemy GetRandomEnemy()
        {
            return _enemies.GetRandom();
        }

        public List<BattleEvent> GetMovements()
        {
            var turn = new List<BattleEvent>();
            
            RefreshEnemies();
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);

            while (_enemyCurrentTurnMoveQueue.Count > 0)
            {
                var enemy = _enemyCurrentTurnMoveQueue.Dequeue();
                
                if (!EnemyIsAlive(enemy)) continue;
                
                // Apply any start-of effects on the enemy
                turn.AddRange(enemy.StartTurn());
                
                // Move sequence
                while (EnemyIsAlive(enemy) && !enemy.FinishedMoving)
                {
                    
                    // Move
                    turn.AddRange(enemy.MoveStep());
                }
                
                // TODO: End of turn processing
                turn.AddRange(enemy.EndTurn());
            }

            return turn;
        }
        
        public List<Enemy> GetEnemiesOnStick(int stick)
        {
            var enemies = new List<Enemy>();
            enemies.AddRange(_enemies.Where(e => e.StickNum == stick));

            return enemies;
        }

        public List<Enemy> GetEnemiesOnSticks(List<int> sticks)
        {
            var enemies = new List<Enemy>();
            foreach (var stick in sticks)
            {
                enemies.AddRange(_enemies.Where(e => e.StickNum == stick));
            }

            return enemies;
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