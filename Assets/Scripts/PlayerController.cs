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
        GameEvents.current.OnEndEnemyTurn += OnEndEnemyTurn;
        GameEvents.current.OnPlayerMovedStick += OnPlayerMovedStick;
        GameEvents.current.OnPlayerBoughtStick += OnPlayerBoughtStick;
        GameEvents.current.OnEnemyReachedEnd += OnEnemyReachedEnd;
        GameEvents.current.OnRedraw += OnRedraw;
        
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
    
    private void OnPlayerBoughtStick()
    {
        UsedMove();
    }

    private void OnEnemyReachedEnd()
    {
        Lives -= 1;
        GameEvents.current.LostLife();
        HUDManager.current.UpdateLives();
        if (Lives <= 0) GameManager.current.OutOfLives();
    }

    private void OnRedraw()
    {
        UsedMove();
    }

    private void UsedMove()
    {
        MovesUsedThisTurn += 1;
    }

    public void PlayCard(int cost)
    {
        for (var i = 0; i < cost; i++)
        {
            UsedMove();
        }
    }
}
