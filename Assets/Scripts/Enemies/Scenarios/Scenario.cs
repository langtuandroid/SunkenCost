using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Grould,
    Claus,
    Axolitl,
    Cucunger,
    Rat,
    FlightingFish,
    Steelo,
    Leech,
    Crib,
    Retortoise,
    Maxolotl,
    Carppool
}

public enum ScenarioType
{
    Easy,
    EasyElite,
    Medium,
    MediumElite,
    Hard,
    HardElite,
}

[CreateAssetMenu(menuName = "Battles/Scenario")]
public class Scenario : ScriptableObject
{
    public ScenarioType scenarioType;

    public List<EnemyType> round1 = new List<EnemyType>();
    public List<EnemyType> round2 = new List<EnemyType>();
    public List<EnemyType> round3 = new List<EnemyType>();
    public List<EnemyType> round4 = new List<EnemyType>();
    public List<EnemyType> round5 = new List<EnemyType>();


    public float scaledDifficulty;

    public List<EnemyType> GetSpawns(int turn)
    {
        if (turn >= RunProgress.PlayerStats.NumberOfTurns) return new List<EnemyType>();
        
        switch (turn)
        {
            case 1: 
                return round1;
            case 2: 
                return round2;
            case 3: 
                return round3;
            case 4: 
                return round4;
            case 5: 
                return round5;
            default:
                Debug.Log("Round " + turn + " not found!");
                return new List<EnemyType>();
        }
    }
}
