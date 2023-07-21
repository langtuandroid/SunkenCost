using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScenarioLoader : MonoBehaviour
{
    public const int BattlesPerDifficulty = 5;
    
    private static ScenarioLoader _current;

    private Dictionary<ScenarioType, List<Scenario>> scenariosByDifficulty { get; set; }= new Dictionary<ScenarioType, List<Scenario>>();
    
    void Awake()
    {
        if (_current)
        {
            Destroy(gameObject);
            return;
        }
        
        _current = this;
        var scenarios = Extensions.LoadScriptableObjects<Scenario>();
        
        foreach (var scenario in scenarios)
        {
            var difficulty = scenario.scenarioType;
            if (scenariosByDifficulty.ContainsKey(difficulty))
            {
                scenariosByDifficulty[difficulty].Add(scenario);
            }
            else
            {
                scenariosByDifficulty.Add(difficulty, new List<Scenario>() {scenario});
            }
        }
        
    }

    public static Scenario GetScenario(int battle)
    {
        ScenarioType scenarioType;

        var stage = (int)Mathf.Floor((float)battle / BattlesPerDifficulty);

        var placeInDifficulty = battle % BattlesPerDifficulty;
        Debug.Log("Battle: " + battle + ", stage: " + stage + ", place:" + placeInDifficulty);

        if (placeInDifficulty == 0)
        {
            scenarioType = (ScenarioType) (9 + stage);
        }
        else
        {
            scenarioType = placeInDifficulty <= ((float)BattlesPerDifficulty / 2) 
                ? (ScenarioType) (stage * 2) : (ScenarioType) ((stage * 2) + 1);
            
        }
        
        Debug.Log(scenarioType);

        var scenarioOptions = _current.scenariosByDifficulty.
            Where(p => p.Key == scenarioType).
            Select(p => p.Value).FirstOrDefault();
        
        if (scenarioOptions?.Count > 0)
        {
            for (var tries = 0; tries < 100; tries++)
            {
                var i = Random.Range(0, scenarioOptions.Count);
                var scenario = scenarioOptions[i];
                
                if (RunProgress.Current.Scenarios.Contains(scenario)) continue;
                scenario.scaledDifficulty = 0;
                RunProgress.Current.Scenarios.Add(scenario);
                return scenario;
            }
        }
        
        throw new Exception($"Couldn't find a scenario that hasn't been used for difficulty {scenarioType}");
    }
}
