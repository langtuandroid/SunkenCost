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
    public static EnemySpawner Instance;
    
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();

    private Scenario _scenario;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }
    }

    public void StartBattle()
    {
        _scenario = ScenarioManager.GetScenario(RunProgress.BattleNumber);
    }

    public List<BattleEvent> SpawnNewTurn()
    {
        var battleEvents = new List<BattleEvent>();
        
        var enemyTypes = _scenario.GetRound(Battle.Current.Turn);
        var newEnemies = enemyTypes.Select(enemyType => Enum.GetName(typeof(EnemyType), enemyType)).ToList();

        if (newEnemies.Count == 0) return battleEvents;

        foreach (var enemyName in newEnemies)
        {
            var enemy = SpawnEnemyOnIsland(enemyName);
            enemy.MaxHealthStat.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.Mover.AddMovementModifier((int)(_scenario.scaledDifficulty / 2f));
            battleEvents.Add(new BattleEvent(BattleEventType.EnemySpawned) {enemyAffectee = enemy});
        }

        return battleEvents;
    }

    public Enemy SpawnEnemyOnIsland(string enemyName)
    {
        var enemy = SpawnEnemy(enemyName, Board.Current.Island);
        enemy.Mover.SetPlankNum(-1);
        return enemy;
    }
    
    public Enemy SpawnEnemyOnPlank(string enemyName, int plankNum)
    {
        var enemy = SpawnEnemy(enemyName, Board.Current.GetPlank(plankNum).transform);
        enemy.Mover.SetPlankNum(plankNum);
        return enemy;
    }

    private Enemy SpawnEnemy(string enemyName, Transform parentTransform)
    {
        var enemyPrefab = enemyDictionary[enemyName];
        var newEnemyObject = Instantiate(enemyPrefab, parentTransform, true);
        newEnemyObject.transform.localPosition = Vector3.zero;
        newEnemyObject.transform.localScale = new Vector3(1, 1, 1);
        
        var newEnemy = newEnemyObject.GetComponent<Enemy>();
        EnemyController.Current.AddEnemy(newEnemy);
        return newEnemy;
    }
}
