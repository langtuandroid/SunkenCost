using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace Enemies
{
    public class EnemySequencer : BattleEventResponder
    {
        public static EnemySequencer Current;
        
        private List<Enemy> _enemies = new List<Enemy>();
        private Queue<Enemy> _enemyCurrentTurnMoveQueue = new Queue<Enemy>();

        public int NumberOfEnemies => _enemies.Count(e => !e.IsDestroyed);
        public ReadOnlyCollection<Enemy> AllEnemies => new ReadOnlyCollection<Enemy>(_enemies);
        
        public bool HasEnemyToMove => _enemyCurrentTurnMoveQueue.Count > 0;

        protected override void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
            base.Awake();
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
            return _enemies.GetRandom(e => !e.IsDestroyed);
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

        public bool GetIfPlankCanBeLandedOn(int plankNum)
        {
            foreach (var landingStopper in GetEnemiesOnPlank(plankNum).OfType<ILandingStopper>())
            {
                if (landingStopper.GetIfStoppingEnemyLandingOnPlank(plankNum)) return false;
            }

            return AllEnemies.Count(e => e.PlankNum == plankNum) < 3;
        }

        public Enemy SelectNextEnemy()
        {
            return _enemyCurrentTurnMoveQueue.Dequeue();
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            switch (previousBattleEvent.type)
            {
                case BattleEventType.EnemyKilled:
                    var deadEnemy = previousBattleEvent.Enemy;

                    foreach (var enemy in _enemies.Where(enemy => enemy.TurnOrder > deadEnemy.TurnOrder))
                    {
                        enemy.SetTurnOrder(enemy.TurnOrder - 1);
                    }
                    
                    RemoveEnemy(deadEnemy);
                    Destroy(deadEnemy.gameObject);
                    break;
                case BattleEventType.StartedEnemyTurn:
                    SetNextEnemyTurnSequence();
                    break;
                case BattleEventType.PlankMoved:
                case BattleEventType.EtchingsOrderChanged:
                case BattleEventType.PlankCreated:
                case BattleEventType.PlankDestroyed:
                    foreach (var e in _enemies)
                        e.RefreshPlankNum();
                    break;
            }

            return BattleEventPackage.Empty;
        }

        private void SetNextEnemyTurnSequence()
        {
            _enemyCurrentTurnMoveQueue = new Queue<Enemy>(_enemies);
        }
    }
}