using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActiveEnemiesManager : MonoBehaviour
{
    public static ActiveEnemiesManager Current { get; private set; }

    private const int EnemiesAllowedOnStick = 3;

    private StickManager _stickManager;

    private List<Enemy> _activeEnemies = new List<Enemy>();
    private List<Enemy> _waitingEnemies = new List<Enemy>();
    private List<Enemy> _allEnemies = new List<Enemy>();

    public List<Enemy> ActiveEnemies => _activeEnemies;
    
    private Enemy[] _enemyTurnOrder = new Enemy[1] {null};
    private int _currentEnemy = 0;
    private bool _canMove = true;
    
    private Dictionary<Stick, List<Enemy>> enemyPositions = new Dictionary<Stick, List<Enemy>>();

    public bool finishedProcessingEnemyTurn = false;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        _stickManager = StickManager.current;
        OldBattleEvents.Current.OnBeginEnemyTurn += BeginEnemyTurn;
        OldBattleEvents.Current.OnEndEnemyTurn += EndEnemyTurn;
        OldBattleEvents.Current.OnEndPlayerTurn += EndPlayerTurn;
    }

    private void Update()
    {
        /*
        if (_enemiesToDestroy.Count > 0)
        {
            foreach (var enemy in _enemiesToDestroy)
            {
                Destroy(enemy.gameObject);
            }
            _enemiesToDestroy.Clear();
        }*/

        if (_activeEnemies.Count > 0 &&
            BattleManager.Current.gameState == GameState.EnemyTurn && _canMove && EtchingManager.Current.finishedProcessingEnemyMove)
        {
           StartCoroutine(MoveNextEnemy());
        }
    }

    public void AddWaitingEnemy(Enemy enemy)
    {
        _waitingEnemies.Add(enemy);
        _allEnemies.Add(enemy);
        ArrangeEnemiesOnStick(enemy);
        RefreshEnemyTurnOrder();
    }

    public void AddActiveEnemy(Enemy enemy)
    {
        _allEnemies.Add(enemy);
        _activeEnemies.Add(enemy);
        ArrangeEnemiesOnStick(enemy);
        RefreshEnemyTurnOrder();
    }

    public static Enemy GetRandomActiveEnemy()
    {
        if (Current.ActiveEnemies.Count == 0) return null;
        return Current.ActiveEnemies[Random.Range(0, Current.ActiveEnemies.Count)];
    }

    private void FindNextEnemyToMove()
    {
        // No enemies left
        if (_activeEnemies.Count == 0)
        {
            finishedProcessingEnemyTurn = true;
            return;
        }

        // Check the next enemy hasn't been destroyed
        for (var i = _currentEnemy; i < _enemyTurnOrder.Length; i++)
        {
            if (_enemyTurnOrder[_currentEnemy] is null || _enemyTurnOrder[_currentEnemy].IsDestroyed)
            {
                _currentEnemy++;
            }
            else break;
        }

        // Check for end of turn
        if (_currentEnemy >= _enemyTurnOrder.Length)
        {
            finishedProcessingEnemyTurn = true;
        }
        else
        {
            // Ready for the next turn
            _canMove = true;
        }
    }

    private IEnumerator MoveNextEnemy()
    {
        FindNextEnemyToMove();
        
        // Can't move any other enemies while this is executing (this is turned back to true after finding next move)
        _canMove = false;
        
        // The enemy to be moved
        var enemy = _enemyTurnOrder[_currentEnemy];

        // Activate any effects the enemy might have
        enemy.BeginMyTurn();
        
        // Wait for the enemy to wiggle
        yield return new WaitForSeconds(0.5f);

        foreach (var action in enemy.PreMovingEffects)
        {
            action.Invoke();
            yield return new WaitForSeconds(BattleManager.AttackTime);
            if (enemy.IsDestroyed) break;
        }

        // If the enemy is not moving, wait for a moment then continue
        if (enemy.FinishedMoving && enemy.PreMovingEffects.Count == 0)
        {
            yield return new WaitForSeconds(BattleManager.AttackTime);
        }
        else
        {
            while (!enemy.FinishedMoving && !enemy.IsDestroyed)
            {
                OldBattleEvents.Current.BegunEnemyMovement();
                
                // Wait for any planks to execute
                while (!EtchingManager.Current.finishedProcessingEnemyMove)
                    yield return 0;
                
                // If the planks have stopped it moving break
                if (enemy.FinishedMoving) break;
                
                StartCoroutine(enemy.ExecuteMoveStep());

                // Wait for the enemy to finish moving
                while (enemy.Moving)
                    yield return 0;

                if (enemy.StickNum == StickManager.current.stickCount) continue;

                OldBattleEvents.Current.CharacterMoved();
                    
                // Wait for any planks to execute
                while (!EtchingManager.Current.finishedProcessingEnemyMove)
                    yield return 0;
            }
        }

        // If the enemy is still alive at the end of the turn
        if (enemy)
        {
            enemy.EndMyTurn();
        }

        _currentEnemy++;

        // Find next enemy
        FindNextEnemyToMove();
    }

    public void EnemyMoved()
    {
        InGameSfxManager.current.EnemyMoved();
        
        // Rearrange the enemies to be neat on the stick
        foreach (var enemy in _allEnemies)
        {
            ArrangeEnemiesOnStick(enemy);
        }
    }

    private void ArrangeEnemiesOnStick(Enemy movingEnemy)
    {
        // Set the parent
        var enemyTransform = movingEnemy.transform;
        enemyTransform.SetParent(_stickManager.stickGrid.transform.GetChild(movingEnemy.StickNum));

        // How many enemies are on this stick?
        var enemiesOnStick =
            _allEnemies.Where(en =>
                (en.StickNum == movingEnemy.StickNum)).ToList();
        
        /*
        var pushEnemies = new List<Enemy>();
        for (var i = movingEnemy.StickNum; )
        for (var ii = 0; enemiesOnStick.Count - ii > EnemiesAllowedOnStick; ii++)
        {
            pushEnemies.Add(enemiesOnStick[0]);
            enemiesOnStick.Remove(enemiesOnStick[0]);
        }

        if (enemiesOnStick.Count > 0)
        {
            StartCoroutine(PushEnemies(pushEnemies));
        }
        */

        var startStickOffsetX = movingEnemy.StickNum == 0 ? 25f : 0f;
        var startStickOffsetY = movingEnemy.StickNum == 0 ? 25f : 0f;
        
        var i = 0f;
        foreach (var e in enemiesOnStick)
        {
            
            
            // Place it a bit further down if there's multiple
            e.RePosition(new Vector3
                (0 + startStickOffsetX, (-120 * i)+ EnemyMover.EnemyOffset + startStickOffsetY));
            i+= e.Size;
        }
    }

    private void BeginEnemyTurn()
    {
        finishedProcessingEnemyTurn = false;

        // Refresh enemy positions
        foreach (var enemiesOnStick in enemyPositions)
        {
            var enemyList = enemiesOnStick.Value;
            var stickNum = enemiesOnStick.Key.GetStickNumber();

            foreach (var enemy in enemyList)
            {
                enemy.Mover.SetStickNum(stickNum);
            }
        }
        
        RefreshEnemyTurnOrder();

        _currentEnemy = 0;

        FindNextEnemyToMove();
    }

    private void EndPlayerTurn()
    {
        // Add next turns enemies to the active list
        foreach (var enemy in _waitingEnemies)
        {
            _activeEnemies.Add(enemy);
        }
        
        _waitingEnemies.Clear();
    }

    private void EndEnemyTurn()
    {
        // Clear the Dictionary;
        enemyPositions.Clear();
        
        // No need if there's no active enemies
        if (_activeEnemies.Count != 0)
        {
            // Save the positions of all the active enemies
            foreach (var enemy in _activeEnemies)
            {
                // Don't save enemies on starting stick
                if (enemy.StickNum == 0) continue;

                // Save all the enemy's positions
                var stick = StickManager.current.stickGrid.GetChild(enemy.StickNum).GetComponent<Stick>();

                if (enemyPositions.ContainsKey(stick))
                {
                    var enemiesOnStick = enemyPositions[stick];
                    enemiesOnStick.Add(enemy);
                    enemyPositions[stick] = enemiesOnStick;
                }
                else
                {
                    enemyPositions[stick] = new List<Enemy>() {enemy};
                }
            }
            
            // Refresh the enemy turn order
            RefreshEnemyTurnOrder();
        }
    }

    public static Enemy CurrentEnemy => Current._enemyTurnOrder[Current._currentEnemy];

    private void RefreshEnemyTurnOrder()
    {
        var numberOfEnemies = _allEnemies.Count;

        // End the turn immediately if there's no enemies
        if (numberOfEnemies == 0)
        {
            finishedProcessingEnemyTurn = true;
            return;
        }

        // Order enemies list
        var newEnemyOrder = new Enemy[numberOfEnemies];
        for (var i = 0; i < numberOfEnemies; i++)
        {
            newEnemyOrder[i] = _allEnemies[i];
            _allEnemies[i].SetTurnOrder(i + 1);
        }

        _enemyTurnOrder = newEnemyOrder;
    }

    public List<Enemy> GetEnemiesOnStick(int stick)
    {
        var enemies = new List<Enemy>();
        enemies.AddRange(_allEnemies.Where(e => e.StickNum == stick));

        return enemies;
    }

    public List<Enemy> GetEnemiesOnSticks(List<int> sticks)
    {
        var enemies = new List<Enemy>();
        foreach (var stick in sticks)
        {
            enemies.AddRange(_allEnemies.Where(e => e.StickNum == stick));
        }

        return enemies;
    }
    
    public void DestroyEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
        _allEnemies.Remove(enemy);
        ArrangeEnemiesOnStick(enemy);
        Destroy(enemy.gameObject);
    }

    public static int NumberOfActiveEnemies => Current._activeEnemies.Count;
    public int NumberOfTotalEnemies => Current._allEnemies.Count;

    public void PlayerKilledEnemy(Enemy enemy)
    {
        DestroyEnemy(enemy);
        BattleManager.Current.AlterGold(enemy.Gold);
    }
}
