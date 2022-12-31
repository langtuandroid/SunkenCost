using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgress
{
    public static int BattleNumber
    {
        get;
        set;
    }

    public static int MaxPlanks
    {
        get;
        set;
    } = 3;

    public static int Lives
    {
        get;
        set;
    } = 3;
    
    public static int MaxLives
    {
        get;
        set;
    } = 3;

    public static int MovesPerTurn { get; set; } = 1;
}
