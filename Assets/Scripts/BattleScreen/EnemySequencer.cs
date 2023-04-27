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
    public class EnemySequencer : BattleEventResponder
    {
        public static EnemySequencer Current;
        
        private List<Enemy> _enemies = new List<Enemy>();
        private Queue<Enemy> _enemyCurrentTurnMoveQueue = new Queue<Enemy>();

        public int NumberOfEnemies => _enemies.Count;
        public ReadOnlyCollection<Enemy> AllEnemies => new ReadOnlyCollection<Enemy>(_enemies);
        
        private bool HasEnemyToMove => _enemyCurrentTurnMoveQueue.Count > 0;
        public Enemy CurrentEnemy { get; private set; }
        public bool HasCurrentEnemy { get; private set; }

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
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
        
        private void SetNextEnemyTurnSequence()
        {
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);
        }

        private void SelectNextEnemy()
        {
            CurrentEnemy = _enemyCurrentTurnMoveQueue.Dequeue();
        }

        private void RefreshEnemies()
        {
            _enemies = _enemies.Where(EnemyIsAlive).ToList();
        }

        private static bool EnemyIsAlive(Enemy enemy)
        {
            return enemy && !enemy.IsDestroyed;
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            switch (previousBattleEvent.type)
            {
                case BattleEventType.StartedNextTurn:
                    SetNextEnemyTurnSequence();
                    break;
                case BattleEventType.PlankMoved:
                    foreach (var enemy in _enemies)
                        enemy.RefreshPlankNum();
                    break;
                case BattleEventType.StartedEnemyMovementPeriod:
                case BattleEventType.EndedIndivdualEnemyMove when HasEnemyToMove:
                    SelectNextEnemy();
                    HasCurrentEnemy = true;
                    return new BattleEventPackage(new BattleEvent(BattleEventType.SelectedNextEnemy) 
                        {enemyAffectee = CurrentEnemy});
                case BattleEventType.EndedIndivdualEnemyMove when !HasEnemyToMove:
                    HasCurrentEnemy = false;
                    break;
            }

            return BattleEventPackage.Empty;
        }
    }
}