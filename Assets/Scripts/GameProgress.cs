using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgress
{
    static GameProgress()
    {
        Reset();
    }
    
    public static void Reset()
    {
        BattleNumber = 0;
        MaxPlanks = 3;
        Lives = 3;
        MovesPerTurn = 1;
        AmountOfCardsToOffer = 2;
    }
    
    public static int BattleNumber
    {
        get;
        set;
    }

    public static int MaxPlanks
    {
        get;
        set;
    }

    public static int Lives
    {
        get;
        set;
    }
    
    public static int MaxLives
    {
        get;
        set;
    }
    public static int MovesPerTurn { get; set; }
    
    public static int AmountOfCardsToOffer
    {
        get;
        set;
    }
}
