using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

public class EnemySpawner : BattleEventResponder
{
    public static EnemySpawner Instance;
    
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();

    private Scenario _scenario;

    private EnemyBattleEventResponderGroup _enemyBattleEventResponderGroup;

    protected override void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        base.Awake();
    }

    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }
        
        _scenario = ScenarioManager.GetScenario(RunProgress.BattleNumber);
    }

    private void OnEnable()
    {
        _enemyBattleEventResponderGroup = FindObjectOfType<EnemyBattleEventResponderGroup>();
    }

    public Enemy SpawnEnemyDuringTurn(string enemyName, int plankNum)
    {
        return plankNum == -1 ? SpawnEnemyOnIsland(enemyName) : SpawnEnemyOnPlank(enemyName, plankNum);
    }

    private BattleEventPackage SpawnNewTurn()
    {
        var enemyTypes = _scenario.GetSpawns(Battle.Current.Turn);
        var newEnemies = enemyTypes.Select(enemyType => Enum.GetName(typeof(EnemyType), enemyType)).ToList();

        if (newEnemies.Count == 0) return BattleEventPackage.Empty;

        var battleEvents = new List<BattleEvent>();

        foreach (var enemyName in newEnemies)
        {
            var enemy = SpawnEnemyOnIsland(enemyName);
            enemy.MaxHealthStat.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.Mover.AddMovementModifier((int) (_scenario.scaledDifficulty / 2f));
            battleEvents.Add(new BattleEvent(BattleEventType.EnemySpawned) {affectedResponderID = enemy.ResponderID});
        }

        return new BattleEventPackage(battleEvents);
    }

    private Enemy SpawnEnemyOnIsland(string enemyName)
    {
        var enemy = SpawnEnemy(enemyName, Board.Current.IslandTransform);
        enemy.Mover.SetPlankNum(-1);
        return enemy;
    }
    
    private Enemy SpawnEnemyOnPlank(string enemyName, int plankNum)
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
        EnemySequencer.Current.AddEnemy(newEnemy);
        return newEnemy;
    }

    public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
    {
        return previousBattleEvent.type == BattleEventType.StartNextPlayerTurn 
            ? SpawnNewTurn() : BattleEventPackage.Empty;
    }
}
