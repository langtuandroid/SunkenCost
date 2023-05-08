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
    public const int BATTLES_PER_DIFFICULTY = 5;
    
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
        switch (battle)
        { 
            case (< BATTLES_PER_DIFFICULTY):
                scenarioType = ScenarioType.Easy;
                break;
            case (BATTLES_PER_DIFFICULTY):
                scenarioType = ScenarioType.EasyElite;
                break;
            case (<BATTLES_PER_DIFFICULTY * 2):
                scenarioType = ScenarioType.Medium;
                break;
            case (BATTLES_PER_DIFFICULTY * 2):
                scenarioType = ScenarioType.MediumElite;
                break;
            case (< BATTLES_PER_DIFFICULTY * 3):
                scenarioType = ScenarioType.Hard;
                break;
            case (BATTLES_PER_DIFFICULTY * 3):
                scenarioType = ScenarioType.HardElite;
                break;
            default:
                Debug.Log(BATTLES_PER_DIFFICULTY);
                scenarioType = ScenarioType.Easy;
                break;
        }

        var scenarioOptions = _current.scenariosByDifficulty.
            Where(p => p.Key == scenarioType).
            Select(p => p.Value).FirstOrDefault();
        
        if (scenarioOptions?.Count > 0)
        {
            var i = Random.Range(0, scenarioOptions.Count);
            var scenario = scenarioOptions[i];
            scenario.scaledDifficulty = 0;
            return scenario;
        }  
        
        //TODO: Get rid of this once all battles have been implemented
        var scaledScenario = GetScenario(0);
        scaledScenario.scaledDifficulty = (int)Math.Floor(battle / 3f);
        return scaledScenario;
    }
}
