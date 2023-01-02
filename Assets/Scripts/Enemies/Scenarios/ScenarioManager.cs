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

    public List<Scenario> scenariosList = new List<Scenario>();
    public Dictionary<int, List<Scenario>> scenarios { get; private set; }= new Dictionary<int, List<Scenario>>();
    
    void Awake()
    {
        if (Current)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
        DontDestroyOnLoad(this.gameObject);
        
        /*
        var scenariosList = AssetDatabase.FindAssets($"t: {nameof(Scenario)}").ToList()
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<Scenario>)
            .ToList();
        */
        
        foreach (var scenario in scenariosList)
        {
            var difficulty = scenario.Difficulty;
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
        var difficulty = (int)Math.Floor(battle / 2f);
        var scenarioOptions = Current.scenarios.Where(p => p.Key == difficulty).Select(p => p.Value).FirstOrDefault();
        if (scenarioOptions?.Count > 0)
        {
            var i = Random.Range(0, scenarioOptions.Count);
            var scenario = scenarioOptions[i];
            scenario.scaledDifficulty = difficulty;
            Debug.Log(scenario.name);
            return scenario;
        }
        
        Debug.Log("Battle No. " + battle + ", difficulty " + difficulty +": No scenario found");
        var scaledScenario = GetScenario(0);
        scaledScenario.scaledDifficulty = (int)Math.Floor(battle / 2f);
        return scaledScenario;
    }
}
