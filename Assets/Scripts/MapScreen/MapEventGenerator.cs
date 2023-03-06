using System;
using System.Collections;
using System.Collections.Generic;
using MapScreen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapEventGenerator : MonoBehaviour
{
    [SerializeField] private MapEvent topMapEvent;
    [SerializeField] private MapEvent bottomMapEvent;
    
    
    private static readonly Dictionary<DisturbanceType, float> Weightings = new Dictionary<DisturbanceType, float>()
    {
        {DisturbanceType.GoldRush, 0.4f},
        {DisturbanceType.Heart, 0.3f},
        {DisturbanceType.UpgradeCard, 0.2f},
        {DisturbanceType.SpecificCard, 0.1f}
    };

    private void Awake()
    {
        var topDisturbance = GenerateDisturbance();
        topMapEvent.disturbance = topDisturbance;

        while (true)
        {
            var bottomDisturbance = GenerateDisturbance();
            if (bottomDisturbance == topDisturbance) continue;
            
            bottomMapEvent.disturbance = bottomDisturbance;
            break;
        }
        
        RunProgress.HaveGeneratedMapEvents();
    }

    private Disturbance GenerateDisturbance()
    {
        var sequence = new[] {
            DisturbanceType.GoldRush,
            DisturbanceType.Heart,
            DisturbanceType.UpgradeCard,
            DisturbanceType.SpecificCard
        };
        
        var rand = Random.value;

        foreach (var disturbanceType in sequence)
        {
            var weighting = Weightings[disturbanceType];

            if (rand <= weighting)
                return DisturbanceManager.GetDisturbance(disturbanceType);

            rand -= weighting;
        }

        Debug.Log("Error: no weighting found for " + rand);
        return null;
    }
}
