using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battles/Scenario")]
public class Scenario : ScriptableObject
{
    public int difficulty;
    public List<string> round1 = new List<string>();
    public List<string> round2 = new List<string>();
    public List<string> round3 = new List<string>();
    public List<string> round4 = new List<string>();
    public List<string> round5 = new List<string>();

    public List<String> GetRound(int round)
    {
        switch (round)
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
                Debug.Log("Round " + round + " not found!");
                return new List<string>();
        }
    }
}
