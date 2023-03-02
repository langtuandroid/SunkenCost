using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Challenges;
using MapScreen;
using OfferScreen;
using UnityEngine;

public static class RunProgress
{
    public static PlayerInventory PlayerInventory;
    public static OfferStorage offerStorage;
    public static int BattleNumber { get; set; }

    public static DisturbanceType currentEvent;
    public static List<Challenge> activeChallenges;
    
    public static bool HasGeneratedMapEvents { get; set; }

    static RunProgress()
    {
        Reset();
    }
    
    public static void Reset()
    {
        PlayerInventory = new PlayerInventory();
        offerStorage = new OfferStorage();
        BattleNumber = 0;
        currentEvent = DisturbanceType.None;
        activeChallenges = new List<Challenge>()
        {
            new CleanSheetChallenge(ChallengeRewardType.Plank)
        };
    }

    public static void SelectNextBattle(DisturbanceType disturbanceDisturbanceType)
    {
        currentEvent = disturbanceDisturbanceType;
        activeChallenges = activeChallenges.Where(c => c.IsActive).ToList();
        Debug.Log(activeChallenges.Count);
    }
}
