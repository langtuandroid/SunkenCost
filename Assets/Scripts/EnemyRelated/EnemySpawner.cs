using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<string> enemyNames = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();
    public Transform startStick;

    private int _round;
    private void Start()
    {
        for (var i = 0; i < enemyNames.Count; i++)
        {
            enemyDictionary.Add(enemyNames[i], enemyPrefabs[i]);
        }

        GameEvents.current.OnEndEnemyTurn += SpawnNewRound;
        GameEvents.current.OnBeginGame += SpawnNewRound;
    }

    private void SpawnNewRound()
    {
        _round = BattleManager.Current.Round + 1;
        
        var newEnemies = GetNewRound();

        // Spawn boss every 16 rounds
        if (_round % 16 == 0)
        {
            newEnemies = new List<string>();
            newEnemies.Add("Boss");
            
            Music.current.SelectSong(2);
        }
        // Only spawn every second round otherwise
        //else if (_round % 2 == 0) return;

        foreach (var enemyName in newEnemies)
        {
            var newEnemy = Instantiate(enemyDictionary[enemyName], startStick, true);
            newEnemy.transform.localPosition = Vector3.zero;
            newEnemy.transform.localScale = new Vector3(1, 1, 1);
            ActiveEnemiesManager.current.AddEnemy(newEnemy.GetComponent<Enemy>());
        }
    }

    private List<string> GetNewRound()
    {
        // For now
        var newEnemies = new List<string>();

            var amountToSpawn = Mathf.Floor(_round / 10f) + 1;
            for (var i = 0; i < amountToSpawn; i++)
            {
                // Count -1 to not include boss
                newEnemies.Add(enemyNames[Random.Range(0, enemyNames.Count-1)]);
            }

            return newEnemies;
    }
}
