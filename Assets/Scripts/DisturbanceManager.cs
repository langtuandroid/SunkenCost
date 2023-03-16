using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class DisturbanceManager : MonoBehaviour
{
    private static DisturbanceManager _current;
    private readonly Dictionary<DisturbanceType, Disturbance> _disturbances = new Dictionary<DisturbanceType, Disturbance>();
    private void Start()
    {
        _current = this;

        var disturbances = Extensions.GetAllInstancesOrNull<Disturbance>();

       foreach (var disturbance in disturbances)
       {
           _disturbances.Add(disturbance.disturbanceType, disturbance);
           
           // Fix the @ symbol in the disturbances description
           disturbance.description = GetDisturbanceDescription(disturbance);
       }
    }

    public static Disturbance GetDisturbance(DisturbanceType disturbanceType)
    {
        return _current._disturbances[disturbanceType];
    }

    public static void ExecuteEndOfBattleDisturbanceAction(DisturbanceType disturbanceType)
    {
        var disturbance = GetDisturbance(disturbanceType);
        
        //TODO: FIX THIS

        switch (disturbance.disturbanceType)
        {
            case DisturbanceType.GoldRush:
                RunProgress.PlayerStats.AlterGold(disturbance.amount);
                break;
            case DisturbanceType.Heart:
                PlayerController.current.AddLife(disturbance.amount);
                break;
            case DisturbanceType.None:
                break;
            case DisturbanceType.UpgradeCard:
                break;
            case DisturbanceType.SpecificCard:
                break;
            case DisturbanceType.Item:
                break;
            case DisturbanceType.Move:
                RunProgress.PlayerStats.MovesPerTurn++;
                break;
            case DisturbanceType.Confused:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private string GetDisturbanceDescription(Disturbance disturbance)
    {
        return disturbance.description.Replace("@", disturbance.amount.ToString());
    }
}
