using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();
    public Transform startStick;

    private Scenario _scenario;
    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }

        BattleEvents.Current.OnBeginPlayerTurn += SpawnNewRound;
        BattleEvents.Current.OnStartBattle += StartBattle;
        
    }

    private void StartBattle()
    {
        _scenario = ScenarioManager.GetScenario(RunProgress.BattleNumber);
        SpawnNewRound();
    }

    private void SpawnNewRound()
    {
        if (BattleManager.Current.Turn == RunProgress.PlayerStats.NumberOfTurns) return;
        
        var enemyTypes = _scenario.GetRound(BattleManager.Current.Turn);
        
        var newEnemies = enemyTypes.Select(enemyType => Enum.GetName(typeof(EnemyType), enemyType)).ToList();

        if (newEnemies.Count == 0) return;

        foreach (var enemyName in newEnemies)
        {
            var newEnemy = Instantiate(enemyDictionary[enemyName], startStick, true);
            newEnemy.transform.localPosition = Vector3.zero;
            newEnemy.transform.localScale = new Vector3(1, 1, 1);

            var enemy = newEnemy.GetComponent<Enemy>();
            enemy.MaxHealth.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.AddMovementModifier((int)(_scenario.scaledDifficulty / 2f));
            ActiveEnemiesManager.Current.AddEnemy(enemy);
        }
    }

    private List<string> GetNewRound()
    {
        // For now
        var newEnemies = new List<string>();

            var amountToSpawn = Mathf.Floor((BattleManager.Current.Turn + (RunProgress.BattleNumber / 3 * 2 * RunProgress.PlayerStats.NumberOfTurns)) / (RunProgress.PlayerStats.NumberOfTurns * 2f)) + 1;
            for (var i = 0; i < amountToSpawn; i++)
            {
                // Count -1 to not include boss
                newEnemies.Add(enemyNames[Random.Range(0, enemyNames.Count-1)]);
            }

            if (Random.Range(0, 3) == 1)
            {
                newEnemies.Clear();
            }

            return newEnemies;
    }
}
