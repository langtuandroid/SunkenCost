using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public int MovesUsedThisTurn { get; private set; } = 0;
    public int MovesPerTurn { get; set; }
    private int _baseMovesPerTurn = 1;

    public int Lives { get; private set; } = 3;
    
    public bool IsOutOfMoves => MovesUsedThisTurn >= MovesPerTurn;
    public int MovesRemaining => MovesPerTurn - MovesUsedThisTurn;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        MovesPerTurn = _baseMovesPerTurn;
    }

    private void Start()
    {
        BattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;
        BattleEvents.Current.OnPlayerMovedStick += OnPlayerMovedStick;
        BattleEvents.Current.OnEnemyReachedEnd += OnEnemyReachedEnd;
        BattleEvents.Current.OnRedraw += OnRedraw;
        
        HUDManager.current.UpdateLives();
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
        Lives -= 1;
        BattleEvents.Current.LostLife();
        HUDManager.current.UpdateLives();
        if (Lives <= 0) BattleManager.Current.OutOfLives();
    }

    private void OnRedraw()
    {
        UsedMove();
    }

    private void UsedMove()
    {
        MovesUsedThisTurn += 1;
        HUDManager.current.UpdateMovesText();
    }

    public void PlayCard(int cost)
    {
        for (var i = 0; i < cost; i++)
        {
            UsedMove();
        }
    }
}
