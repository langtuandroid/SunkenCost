using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace BattleScreen
{
    public class EnemySequencer : MonoBehaviour
    {
        public static EnemySequencer Current;
        
        private List<Enemy> _enemies = new List<Enemy>();
        private Queue<Enemy> _enemyCurrentTurnMoveQueue = new Queue<Enemy>();

        public int NumberOfEnemies => _enemies.Count;
        public ReadOnlyCollection<Enemy> AllEnemies => new ReadOnlyCollection<Enemy>(_enemies);
        public bool HasEnemyToMove => _enemyCurrentTurnMoveQueue.Count > 0;

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
        }

        public void SetNextEnemyTurnSequence()
        {
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);
        }

        public Enemy GetNextEnemyToMove()
        {
            return _enemyCurrentTurnMoveQueue.Dequeue();
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            
            // This will be overwritten if this function is not called during the enemy move period
            _enemyCurrentTurnMoveQueue.Enqueue(enemy);
            
            enemy.SetTurnOrder(_enemies.Count);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemyCurrentTurnMoveQueue.Where(e => e != enemy));
            enemy.SetTurnOrder(_enemies.Count);
        }
        
        public Enemy GetRandomEnemy()
        {
            return _enemies.GetRandom();
        }

        /*
        public List<BattleEvent> GetMovements()
        {
            var turn = new List<BattleEvent>();
            
            RefreshEnemies();
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);
            var enemyCount = _enemies.Count;
            var currentEnemyTurnOrder = 1;

            while (_enemyCurrentTurnMoveQueue.Count > 0)
            {
                var enemy = _enemyCurrentTurnMoveQueue.Dequeue();

                if (!EnemyIsAlive(enemy)) continue;
                
                enemy.SetTurnOrder(currentEnemyTurnOrder);
                
                // Apply any start-of effects on the enemy
                turn.AddRange(enemy.StartTurn());
                
                // Move sequence
                while (EnemyIsAlive(enemy) && !enemy.FinishedMoving)
                {
                    // Move
                    turn.AddRange(enemy.MoveStep());
                }
                
                if (EnemyIsAlive(enemy)) turn.AddRange(enemy.EndTurn());

                currentEnemyTurnOrder++;
            }

            return turn;
        }
        */
        
        public List<Enemy> GetEnemiesOnPlank(int plankNum)
        {
            var enemies = new List<Enemy>();
            enemies.AddRange(_enemies.Where(e => e.PlankNum == plankNum));

            return enemies;
        }

        public List<Enemy> GetEnemiesOnPlanks(List<int> plankNums)
        {
            var enemies = new List<Enemy>();
            foreach (var plankNum in plankNums)
            {
                enemies.AddRange(_enemies.Where(e => e.PlankNum == plankNum));
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