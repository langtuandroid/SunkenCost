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
}
