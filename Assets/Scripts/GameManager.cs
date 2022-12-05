using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Items;
using UnityEngine.SceneManagement;

public enum GameState
{
    PlayerTurn,
    EnemyTurn,
    OfferingDesigns
}

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    
    public GameState gameState = GameState.PlayerTurn;
    public const float AttackTime = 0.6f;
    
    private Random _random = new Random();

    public int Round { get; private set; } = 0;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    private void Start()
    {
        GlobalEvents.current.LoadedLevel();
        
        GameEvents.current.OnDesignOfferAccepted += DesignOfferAccepted;
        GameEvents.current.OnOfferDesigns += OfferingDesigns;
        GameEvents.current.OnBossKilled += BossKilled;
        
        GameEvents.current.BeginGame();
        Round = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //
        if (Input.GetKeyDown(KeyCode.X))
        {
            ItemManager.current.EquipItem("ExtraTurn");
            InventoryManager.current.AlterGold(50);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            StickManager.current.DestroyStick(1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SkipToBoss();
        }

        // Begin Next turn
        if (Input.GetKeyDown(KeyCode.N))
        {
            TryNextTurn();
        }
        //

        if (gameState == GameState.EnemyTurn && EtchingManager.current.finishedProcessingEnemyMove && ActiveEnemiesManager.current.finishedProcessingEnemyTurn)
        {
            GameEvents.current.EndEnemyTurn();
            BeginPlayerTurn();
        }

    }

    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Music.current.SelectSong(0);
    }

    public bool TryNextTurn()
    {
        // LATER - chane to just NoTurn
        if (gameState == GameState.PlayerTurn)
        {
            GameEvents.current.EndPlayerTurn();
            WhoseTurnText.current.EnemiesTurn();
            NextRoundButton.current.CanClick(false);
            InGameSfxManager.current.NextTurn();

            // Give the game a frame to catch up
            StartCoroutine(ProcessTurnChangeover());

            return true;
        }
        
        return false;
    }

    private void BeginPlayerTurn()
    {
        gameState = GameState.PlayerTurn;
        InGameSfxManager.current.BeginTurn();
        WhoseTurnText.current.PlayersTurn();
            
        GameEvents.current.BeginPlayerTurn();
            
        NextRoundButton.current.CanClick(true);
            
        Round++;
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

    public void SkipToBoss()
    {
        var alterAmount = 16 / Round;
        if (alterAmount > 10) alterAmount = 10;
        InventoryManager.current.AlterGold(alterAmount);
        Round = 15;
    }

    public void OutOfLives()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Music.current.SelectSong(0);
    }

    private IEnumerator ProcessTurnChangeover()
    {
        yield return new WaitForSeconds(0.3f);
        gameState = GameState.EnemyTurn;
        GameEvents.current.BeginEnemyTurn();
    }
}
