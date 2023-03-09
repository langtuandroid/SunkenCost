using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Challenges;
using Challenges.Challenges;
using MapScreen;
using OfferScreen;
using UnityEngine;

public class RunProgress : MonoBehaviour
{
    private static RunProgress _current;
    private PlayerProgress _playerProgress;
    private OfferStorage _offerStorage;

    private int _battleNumber;

    private DisturbanceType _currentEvent;
    private List<Challenge> _activeChallenges;

    private bool _hasGeneratedMapEvents;

    public static PlayerProgress PlayerProgress => _current._playerProgress;
    public static OfferStorage OfferStorage => _current._offerStorage;
    
    public static int BattleNumber => _current._battleNumber;

    public static DisturbanceType CurrentEvent => _current._currentEvent;
    
    public static List<Challenge> ActiveChallenges => _current._activeChallenges;

    public static bool HasGeneratedMapEvents => _current._hasGeneratedMapEvents;

    public void Awake()
    {
        _current = this;
    }

    public static void Initialise()
    {
        _current.InitialiseRun();
    }

    public static void SelectNextBattle(DisturbanceType disturbanceDisturbanceType)
    {
        _current._currentEvent = disturbanceDisturbanceType;
        _current._activeChallenges = ActiveChallenges.Where(c => c.IsActive).ToList();
        _current._battleNumber++;
        _current._hasGeneratedMapEvents = false;
    }

    public static void HaveGeneratedMapEvents()
    {
        _current._hasGeneratedMapEvents = true;
    }

    public static Challenge[] ExtractCompletedChallenges()
    {
        var completedChallenges = ActiveChallenges.Where(c => c.HasAchievedCondition()).ToArray();

        foreach (var challenge in completedChallenges)
        {
            ActiveChallenges.Remove(challenge);
        }

        return completedChallenges;
    }
    
    private void InitialiseRun()
    {
        _playerProgress = new PlayerProgress();
        _playerProgress.InitialiseDeck();
        _playerProgress.InitialiseItems();
        _offerStorage = new OfferStorage();
        _battleNumber = 0;
        _currentEvent = DisturbanceType.None;
        _activeChallenges = new List<Challenge>();
    }
}
