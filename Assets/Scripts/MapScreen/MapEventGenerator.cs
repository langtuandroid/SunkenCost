using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapScreen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapEventGenerator : MonoBehaviour
{
    [SerializeField] private MapEvent topMapEvent;
    [SerializeField] private MapEvent bottomMapEvent;
    
    
    private static readonly Dictionary<DisturbanceType, float> NormalWeightings = new Dictionary<DisturbanceType, float>()
    {
        {DisturbanceType.GoldRush, 0.4f},
        {DisturbanceType.Heart, 0.3f},
        {DisturbanceType.UpgradeCard, 0.2f},
        {DisturbanceType.SpecificCard, 0.1f}
    };
    
    private static readonly Dictionary<DisturbanceType, float> EliteWeightings = new Dictionary<DisturbanceType, float>()
    {
        {DisturbanceType.Confused, 0.5f},
        {DisturbanceType.Move, 0.5f}
    };

    private void Awake()
    {
        var eliteRound = RunProgress.BattleNumber % 3 == 2;
        
        var topDisturbance = GenerateDisturbance(eliteRound);
        topMapEvent.disturbance = topDisturbance;
        topMapEvent.isElite = eliteRound;

        for (var i = 0; i < 1000; i++)
        {
            if (i == 999)
            {
                Debug.Log("Couldn't generate a disturbance!!");
            }
            
            var bottomDisturbance = GenerateDisturbance(eliteRound);
            if (bottomDisturbance == topDisturbance) continue;
            
            bottomMapEvent.disturbance = bottomDisturbance;
            bottomMapEvent.isElite = eliteRound;
            break;
        }
        
        RunProgress.HaveGeneratedMapEvents();
    }

    private Disturbance GenerateDisturbance(bool eliteRound)
    {
        var sequence = eliteRound
            ? EliteWeightings
            : NormalWeightings;
        
        var rand = Random.value;

        foreach (var kvp in sequence)
        {
            var weighting = kvp.Value;

            if (rand <= weighting)
                return DisturbanceManager.GetDisturbance(kvp.Key);

            rand -= weighting;
        }

        Debug.Log("Error: no weighting found for " + rand);
        return null;
    }
}
