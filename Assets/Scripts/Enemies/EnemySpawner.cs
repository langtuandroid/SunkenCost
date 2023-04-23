using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using Enemies;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Current;
    
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();

    private Scenario _scenario;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }
    }

    private void StartBattle()
    {
        _scenario = ScenarioManager.GetScenario(RunProgress.BattleNumber);
        SpawnNewRound();
    }

    public Enemy SpawnEnemy(string enemyName, int plankNum)
    {
        var enemyPrefab = enemyDictionary[enemyName];
        var stickToSpawnOn = Board.Current.GetPlank(plankNum).transform;
        var newEnemyObject = Instantiate(enemyPrefab, stickToSpawnOn, true);
        newEnemyObject.transform.localPosition = Vector3.zero;
        newEnemyObject.transform.localScale = new Vector3(1, 1, 1);
        
        var newEnemy = newEnemyObject.GetComponent<Enemy>();
        newEnemy.Mover.SetStickNum(plankNum);
        EnemyController.Current.AddEnemy(newEnemy);
        return newEnemy;
    }

    private void SpawnNewRound()
    {
        if (Battle.Current.Turn == RunProgress.PlayerStats.NumberOfTurns) return;
        
        var enemyTypes = _scenario.GetRound(Battle.Current.Turn);
        
        var newEnemies = enemyTypes.Select(enemyType => Enum.GetName(typeof(EnemyType), enemyType)).ToList();

        if (newEnemies.Count == 0) return;

        foreach (var enemyName in newEnemies)
        {
            var enemy = SpawnEnemy(enemyName, 0);
            enemy.MaxHealthStat.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.Mover.AddMovementModifier((int)(_scenario.scaledDifficulty / 2f));
        }
    }
}
