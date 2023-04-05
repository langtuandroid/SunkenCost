using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner current;
    
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();

    private Scenario _scenario;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }

        OldBattleEvents.Current.OnBeginPlayerTurn += SpawnNewRound;
        OldBattleEvents.Current.OnStartBattle += StartBattle;
        
    }

    private void StartBattle()
    {
        _scenario = ScenarioManager.GetScenario(RunProgress.BattleNumber);
        SpawnNewRound();
    }

    public Enemy SpawnActiveEnemy(string enemyName, int stickNum)
    {
        var newEnemy = SpawnEnemy(enemyName, stickNum);
        ActiveEnemiesManager.Current.AddActiveEnemy(newEnemy);
        return newEnemy;
    }
    
    private Enemy SpawnWaitingEnemy(string enemyName)
    {
        var newEnemy = SpawnEnemy(enemyName, 0);
        ActiveEnemiesManager.Current.AddWaitingEnemy(newEnemy);
        return newEnemy;
    }

    private Enemy SpawnEnemy(string enemyName, int stickNum)
    {
        var enemyPrefab = enemyDictionary[enemyName];
        var stickToSpawnOn = StickManager.current.stickGrid.GetChild(stickNum);
        var newEnemyObject = Instantiate(enemyPrefab, stickToSpawnOn, true);
        newEnemyObject.transform.localPosition = Vector3.zero;
        newEnemyObject.transform.localScale = new Vector3(1, 1, 1);
        
        var newEnemy = newEnemyObject.GetComponent<Enemy>();
        newEnemy.Mover.SetStickNum(stickNum);
        return newEnemy;
    }

    private void SpawnNewRound()
    {
        if (BattleManager.Current.Turn == RunProgress.PlayerStats.NumberOfTurns) return;
        
        var enemyTypes = _scenario.GetRound(BattleManager.Current.Turn);
        
        var newEnemies = enemyTypes.Select(enemyType => Enum.GetName(typeof(EnemyType), enemyType)).ToList();

        if (newEnemies.Count == 0) return;

        foreach (var enemyName in newEnemies)
        {
            var enemy = SpawnWaitingEnemy(enemyName);
            enemy.MaxHealth.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.Mover.AddMovementModifier((int)(_scenario.scaledDifficulty / 2f));
        }
    }
}
