using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScene;
using BattleScreen;
using Disturbances;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Items;
using UnityEngine.SceneManagement;

/*

public class BattleManager : MonoBehaviour
{
    private Challenge[] _completedChallenges;
    [SerializeField] private EndOfBattlePopup endOfBattlePopup;
    
    public static BattleManager Current;

    public GameState gameState = GameState.PlayerTurn;
    public const float AttackTime = 0.6f;

    private Random _random = new Random();

    public int Turn { get; private set; } = 1;

    private void Awake()
    {
        // One instance of static objects only
        if (Current)
        {
            Destroy(gameObject);
            return;
        }

        Current = this;
    }

    private void Start()
    {
        GlobalEvents.current.LoadedLevel();

        OldBattleEvents.Current.OnDesignOfferAccepted += DesignOfferAccepted;
        OldBattleEvents.Current.OnOfferDesigns += OfferingDesigns;
        OldBattleEvents.Current.OnBossKilled += BossKilled;

        foreach (var design in RunProgress.PlayerStats.Deck)
        {
            PlankMap.current.CreateStick();
            EtchingMap.Current.CreateEtching(PlankMap.current.GetStick(PlankMap.current.stickCount - 1),
                design);
        }
        
        OldBattleEvents.Current.BeginBattle();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //
        if (Input.GetKeyDown(KeyCode.X))
        {
            RunProgress.PlayerStats.AlterGold(50);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlankMap.current.DestroyStick(1);
        }

        // Begin Next turn
        if (Input.GetKeyDown(KeyCode.N))
        {
            TryNextTurn();
        }
        //

        if (gameState == GameState.EnemyTurn && EtchingMap.Current.finishedProcessingEnemyMove &&
            ActiveEnemiesManager.Current.finishedProcessingEnemyTurn)
        {
            OldBattleEvents.Current.EndEnemyTurn();
            
            if (Turn >= RunProgress.PlayerStats.NumberOfTurns)
            {
                gameState = GameState.Rewards;
                CreateEndOfBattlePopup();
            }
            else
            {
                BeginPlayerTurn();
            }
        }

    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
        Music.current.SelectSong(0);
    }

    public bool TryNextTurn()
    {
        // LATER - chane to just NoTurn
        if (gameState == GameState.PlayerTurn)
        {
            OldBattleEvents.Current.EndPlayerTurn();
            NextTurnButton.Current.CanClick(false);
            InGameSfxManager.current.NextTurn();
            
            // Give the game a frame to catch up
            StartCoroutine(ProcessTurnChangeover());

            return true;
        }

        return false;
    }

    public void AlterGold(int amount)
    {
        RunProgress.PlayerStats.AlterGold(amount);
        OldBattleEvents.Current.PlayerGainedGold();
    }

    private void BeginPlayerTurn()
    {
        gameState = GameState.PlayerTurn;
        InGameSfxManager.current.BeginTurn();

        Turn++;
        OldBattleEvents.Current.BeginPlayerTurn();

        NextTurnButton.Current.CanClick(true);
    }

    private void OfferingDesigns()
    {
        gameState = GameState.OfferingDesigns;
    }

    private void DesignOfferAccepted()
    {
        gameState = GameState.PlayerTurn;
        BeginPlayerTurn();
    }

    private void BossKilled()
    {
        Music.current.SelectSong(1);
    }

    public void OutOfLives()
    {
        SceneManager.LoadScene(0);
        Music.current.SelectSong(0);
    }
    
    public void CreateEndOfBattlePopup()
    {
        endOfBattlePopup.gameObject.SetActive(true);
        var disturbance = RunProgress.CurrentDisturbance;
        endOfBattlePopup.SetReward(disturbance);
        endOfBattlePopup.SetButtonAction(SwapToChallengeRewardsOrEndBattle);
        
    }

    private void SwapToChallengeRewardsOrEndBattle()
    {
        OldBattleEvents.Current.EndBattle();
        
        _completedChallenges = RunProgress.ExtractCompletedChallenges();

        if (_completedChallenges.Length > 0)
        {
            endOfBattlePopup.SwapToChallengeRewards(_completedChallenges);
            endOfBattlePopup.SetButtonAction(AcceptChallengeRewards);
        }
        else
        {
            EndBattle();
        }
    }

    private void AcceptChallengeRewards()
    {
        foreach (var challenge in _completedChallenges)
        {
            RunProgress.PlayerStats.AcceptChallengeReward(challenge.ChallengeRewardType);
        }
        
        EndBattle();
    }

    private void EndBattle()
    {
        DisturbanceManager.ExecuteEndOfBattleDisturbanceAction(RunProgress.CurrentDisturbance);
        
        RunProgress.PlayerStats.Lives = PlayerController.current.Lives;
        MainManager.Current.LoadOfferScreen();
        Destroy(gameObject);
    }

    private IEnumerator ProcessTurnChangeover()
    {
        yield return new WaitForSeconds(0.3f);
        gameState = GameState.EnemyTurn;
        OldBattleEvents.Current.BeginEnemyTurn();
    }
}

*/

