using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class DisturbanceManager : MonoBehaviour
{
    private static DisturbanceManager _current;
    private readonly Dictionary<DisturbanceType, Disturbance> Disturbances = new Dictionary<DisturbanceType, Disturbance>();
    private void Start()
    {
        _current = this;
        
       var guids = AssetDatabase.FindAssets("t:" + nameof(Disturbance));
       var disturbances = new Disturbance[guids.Length];

       for (var i = 0; i < guids.Length; i++)
       {
           var path = AssetDatabase.GUIDToAssetPath(guids[i]);
           disturbances[i] = AssetDatabase.LoadAssetAtPath<Disturbance>(path);
       }

       foreach (var disturbance in disturbances)
       {
           Disturbances.Add(disturbance.disturbanceType, disturbance);
           
           // Fix the @ symbol in the disturbances description
           disturbance.description = GetDisturbanceDescription(disturbance);
       }
    }

    public static Disturbance GetDisturbance(DisturbanceType disturbanceType)
    {
        return _current.Disturbances[disturbanceType];
    }

    public static void ExecuteEndOfBattleDisturbanceAction(DisturbanceType disturbanceType)
    {
        var disturbance = GetDisturbance(disturbanceType);

        if (disturbance.disturbanceType == DisturbanceType.GoldRush)
            RunProgress.PlayerInventory.AlterGold(disturbance.amount);
        else if (disturbance.disturbanceType == DisturbanceType.Heart)
            PlayerController.current.AddLife(disturbance.amount);
    }

    private string GetDisturbanceDescription(Disturbance disturbance)
    {
        return disturbance.description.Replace("@", disturbance.amount.ToString());
    }
}
