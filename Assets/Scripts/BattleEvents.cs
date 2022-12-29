using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Etchings;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Current;

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

    public event Action OnBeginGame;
    
    public event Action OnEndEnemyTurn;
    public event Action OnEndPlayerTurn;
    public event Action OnBeginPlayerTurn;
    public event Action OnPlayerMovedStick;
    public event Action OnStickAdded;
    public event Action OnBeginEnemyTurn;
    public event Action OnOfferDesigns;
    public event Action OnDesignOfferAccepted;
    public event Action OnSticksUpdated;
    
    
    public event Action OnLostLife;
    public event Action OnRedraw;
    public event Action OnLevelUp;
    
    
    public event Action OnBossSpawned;
    public event Action OnBossKilled;
    public event Action OnEnemyReachedEnd;
    public event Action OnBeginEnemyMove;
    public event Action OnEnemyMoved;
    public event Action OnEnemyAttacked;

    private Enemy _lastEnemyAttacked;
    private Etching _lastEtchingToAttack;
    public static Enemy LastEnemyAttacked => Current._lastEnemyAttacked;
    public static Etching LastEtchingToAttack => Current._lastEtchingToAttack;

    public void BeginGame()
    {
        OnBeginGame?.Invoke();
    }

    public void BegunEnemyMovement()
    {
        OnBeginEnemyMove?.Invoke();
    }

    public void CharacterMoved()
    {
        OnEnemyMoved?.Invoke();
    }

    public void EndEnemyTurn()
    {
        Log.current.AddEvent("ENEMY TURN ENDS");
        OnEndEnemyTurn?.Invoke();
    }
    
    public void EndPlayerTurn()
    {
        Log.current.AddEvent("PLAYER TURN ENDS");
        OnEndPlayerTurn?.Invoke();
    }
    
    public void BeginEnemyTurn()
    {
        Log.current.AddEvent("ENEMY TURN BEGINS");
        OnBeginEnemyTurn?.Invoke();
    }
    public void BeginPlayerTurn()
    {
        Log.current.AddEvent("PLAYER TURN BEGINS");
        OnBeginPlayerTurn?.Invoke();
    }
    
    public void PlayerMovedStick()
    {
        StartCoroutine(WaitForStickRefresh());
    }

    private IEnumerator WaitForStickRefresh()
    {
        yield return 0;
        Log.current.AddEvent("PLAYER MOVED STICK");
        OnPlayerMovedStick?.Invoke();
        EtchingsUpdated();
    }
    
    public void StickAdded()
    {
        Log.current.AddEvent("STICK ADDED");
        OnStickAdded?.Invoke();
    }

    public void OfferDesigns()
    {
        Log.current.AddEvent("OFFERING DESIGNS");
        OnOfferDesigns?.Invoke();
    }
    
    public void DesignOfferAccepted()
    {
        Log.current.AddEvent("OFFER ACCEPTED");
        OnDesignOfferAccepted?.Invoke();
    }

    public void EtchingsUpdated()
    {
        Log.current.AddEvent("STICK MOVED");
        StartCoroutine(GiveSticksTimeToLoad());

    }

    public void EnemyReachedEnd()
    {
        Log.current.AddEvent("ENEMY REACHED END");
        OnEnemyReachedEnd?.Invoke();
    }

    public void BossSpawned()
    {
        OnBossSpawned?.Invoke();
    }

    public void BossKilled()
    {
        Log.current.AddEvent("BOSS KILLED");
        OnBossKilled?.Invoke();
    }

    public void LostLife()
    {
        OnLostLife?.Invoke();
    }

    public void Redrew()
    {
        OnRedraw?.Invoke();
    }

    public void LevelUp()
    {
        OnLevelUp?.Invoke();
    }

    public void EnemyAttacked(Enemy enemy, Etching etching)
    {
        Current._lastEnemyAttacked = enemy;
        Current._lastEtchingToAttack = etching;
        OnEnemyAttacked?.Invoke();
    }

    private IEnumerator GiveSticksTimeToLoad()
    {
        yield return 0;
        OnSticksUpdated?.Invoke();
    }
}
