using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies;
using Enemies.Enemies;
using UnityEngine;

public class EnemySpawner : BattleEventResponder
{
    public static EnemySpawner Instance;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _elitePrefab;

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

    public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
    {
        return new List<BattleEventResponseTrigger>
        {
            PackageResponseTrigger(BattleEventType.StartedNextPlayerTurn, e => SpawnNewTurn())
        };
    }

    private void Start()
    {
        _scenario = ScenarioLoader.GetScenario(RunProgress.Current.BattleNumber);
    }

    private void OnEnable()
    {
        _enemyBattleEventResponderGroup = FindObjectOfType<EnemyBattleEventResponderGroup>();
    }

    public Enemy SpawnEnemyDuringTurn(EnemyType enemyType, int plankNum)
    {
        return plankNum == -1 ? SpawnEnemyOnIsland(enemyType) : SpawnEnemyOnPlank(enemyType, plankNum);
    }

    private BattleEventPackage SpawnNewTurn()
    {
        var enemyTypes = _scenario.GetSpawns(Battle.Current.Turn);
        
        // TODO: Move this somewhere sexier
        if (RunProgress.Current.RunthroughStartingConfig.IsActive)
        {
            var startingEnemies = RunProgress.Current.RunthroughStartingConfig.StartingEnemies;
            if (startingEnemies is not null && startingEnemies.Length > 0) enemyTypes = startingEnemies.ToList();
        }

        if (enemyTypes.Count == 0) return BattleEventPackage.Empty;

        var battleEvents = new List<BattleEvent>();

        foreach (var enemyType in enemyTypes)
        {
            var enemy = SpawnEnemyOnIsland(enemyType);
            enemy.MaxHealthStat.AddModifier(new StatModifier(_scenario.scaledDifficulty, StatModType.PercentMult));
            enemy.Mover.AddMovementModifier((int) (_scenario.scaledDifficulty / 2f)
                                            + RunProgress.Current.PlayerStats.EnemyMovementModifier);
            battleEvents.Add(new BattleEvent(BattleEventType.EnemySpawned)
            {
                primaryResponderID = enemy.ResponderID, 
                showResponse =  false
            });
        }

        return new BattleEventPackage(battleEvents);
    }

    private Enemy SpawnEnemyOnIsland(EnemyType enemyType)
    {
        var enemy = SpawnEnemy(enemyType, Board.Current.IslandTransform);
        enemy.Initialise(-1);
        return enemy;
    }
    
    private Enemy SpawnEnemyOnPlank(EnemyType enemyType, int plankNum)
    {
        var enemy = SpawnEnemy(enemyType, Board.Current.GetPlank(plankNum).transform);
        enemy.Initialise(plankNum);
        return enemy;
    }

    private Enemy SpawnEnemy(EnemyType enemyType, Transform parentTransform)
    {
        var enemyAsset = EnemyLoader.EnemyAssets.First(a => a.EnemyType == enemyType);
        var enemyClass = enemyAsset.Class;
        var prefab = enemyClass.IsSubclassOf(typeof(EliteEnemy)) ? _elitePrefab : _enemyPrefab;

        var newEnemyObject = Instantiate(prefab, parentTransform, true);
        newEnemyObject.transform.localPosition = Vector3.zero;
        newEnemyObject.transform.localScale = new Vector3(1, 1, 1);

        var newEnemy = newEnemyObject.AddComponent(enemyClass).GetComponent<Enemy>();
        EnemySequencer.Current.AddEnemy(newEnemy);
        return newEnemy;
    }
}
