using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public int MovesUsedThisTurn { get; private set; } = 0;
    public int MovesPerTurn { get; set; }
    private int _baseMovesPerTurn;

    public int Lives { get; private set; }
    private int MaxLives { get; set; }
    
    public bool IsOutOfMoves => MovesUsedThisTurn >= MovesPerTurn;
    public int MovesRemaining => MovesPerTurn - MovesUsedThisTurn;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(current.gameObject);
        }
        
        current = this;
        
        Lives = RunProgress.PlayerStats.Lives;
        MaxLives = RunProgress.PlayerStats.MaxLives;
        _baseMovesPerTurn = RunProgress.PlayerStats.MovesPerTurn;
        MovesPerTurn = _baseMovesPerTurn;
    }

    private void Start()
    {
        OldBattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;
        OldBattleEvents.Current.OnPlayerMovedStick += OnPlayerMovedStick;
        OldBattleEvents.Current.OnEnemyReachedEnd += OnEnemyReachedEnd;
        OldBattleEvents.Current.OnRedraw += OnRedraw;

        BattleHUDManager.current.UpdateMovesText();
    }

    private void OnEndEnemyTurn()
    {
        MovesUsedThisTurn = 0;
    }

    private void OnPlayerMovedStick()
    {
        UsedMove();
    }

    private void OnEnemyReachedEnd()
    {
        TakeLife();
    }

    public void TakeLife()
    {
        Lives -= 1;
        OldBattleEvents.Current.LostLife();
        if (Lives <= 0) BattleManager.Current.OutOfLives();
    }

    public void AddLife(int amount)
    {
        Lives += amount;
        if (Lives > MaxLives) Lives = MaxLives;
    }

    private void OnRedraw()
    {
        UsedMove();
    }

    private void UsedMove()
    {
        MovesUsedThisTurn += 1;
        BattleHUDManager.current.UpdateMovesText();
    }

    public void PlayCard(int cost)
    {
        for (var i = 0; i < cost; i++)
        {
            UsedMove();
        }
    }
}
