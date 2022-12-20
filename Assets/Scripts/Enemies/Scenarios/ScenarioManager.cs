using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScenarioManager : MonoBehaviour
{
    private static ScenarioManager Current;
    public Dictionary<int, List<Scenario>> scenarios { get; private set; }= new Dictionary<int, List<Scenario>>();
    
    void Awake()
    {
        if (Current)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
        
        var scenariosList = AssetDatabase.FindAssets($"t: {nameof(Scenario)}").ToList()
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Scenario>)
            .ToList();

        foreach (var scenario in scenariosList)
        {
            var difficulty = scenario.difficulty;
            if (scenarios.ContainsKey(difficulty))
            {
                scenarios[difficulty].Add(scenario);
            }
            else
            {
                scenarios.Add(difficulty, new List<Scenario>() {scenario});
            }
        }
    }

    public static Scenario GetScenario(int battle)
    {
        var difficulty = (int)Math.Floor(battle / 5f);
        var scenarioOptions = Current.scenarios.Where(p => p.Key == difficulty).Select(p => p.Value).FirstOrDefault();
        Debug.Log(scenarioOptions?.Count);
        if (scenarioOptions != null)
        {
            return scenarioOptions[Random.Range(0, scenarioOptions.Count)];
        }
        
        Debug.Log("Battle No. " + battle + ", difficulty " + difficulty +": No scenario found");
        return null;
    }
}
