using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;

public class InGameSfxManager : MonoBehaviour
{
    public static InGameSfxManager current;

    private AudioSource _audioSource;

    [SerializeField] private StudioEventEmitter _successfulBuyPlankSound;
    [SerializeField] private StudioEventEmitter _goodClickSound;
    [SerializeField] private StudioEventEmitter _badClickSound;
    [FormerlySerializedAs("_stickMovementSound")] [SerializeField] private StudioEventEmitter _plankMovementSound;
    [SerializeField] private StudioEventEmitter _charMovementSound;
    [SerializeField] private StudioEventEmitter _damageSound;
    [SerializeField] private StudioEventEmitter _slimedSound;
    [SerializeField] private StudioEventEmitter _destroyedPlankSound;
    [SerializeField] private StudioEventEmitter _poisonSound;
    [SerializeField] private StudioEventEmitter _healSound;
    [SerializeField] private StudioEventEmitter _deathSound;
    [SerializeField] private StudioEventEmitter _nextTurnSound;
    [SerializeField] private StudioEventEmitter _beginTurnSound;
    
    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void TriggerAudio(BattleEventPackage battleEventPackage)
    {
        foreach (var battleEvent in battleEventPackage.battleEvents)
        {
            switch (battleEvent.type)
            {
                case BattleEventType.EnemyAttacked:
                    AttackedEnemy();
                    break;
                case BattleEventType.EtchingStunned:
                    Slimed();
                    break;
                case BattleEventType.EnemyMove:
                    EnemyMoved();
                    break;
                case BattleEventType.EnemyPoisoned:
                    Poisoned();
                    break;
                case BattleEventType.EnemyHealed:
                    Healed();
                    break;
                case BattleEventType.EnemyKilled:
                    Death();
                    break;
                case BattleEventType.PlankDestroyed:
                    DestroyedPlank();
                    break;
                case BattleEventType.PlankMoved:
                    MovedPlank();
                    break;
                case BattleEventType.PlankCreated:
                    CreatedPlank();
                    break;
                case BattleEventType.StartNextPlayerTurn:
                    StartPlayerTurn();
                    break;
                case BattleEventType.StartedEnemyTurn:
                    StartEnemyTurn();
                    break;
            }
        }
    }
    
    public void GoodClick()
    {
        _goodClickSound.Play();
    }

    public void BadClick()
    {
        _badClickSound.Play();
    }
    
    private void CreatedPlank()
    {
        _successfulBuyPlankSound.Play();
    }

    private void MovedPlank()
    {
        _plankMovementSound.Play();
    }

    private void EnemyMoved()
    {
        _charMovementSound.Play();
    }

    private void AttackedEnemy()
    {
        _damageSound.Play();
    }

    private void Slimed()
    {
        _slimedSound.Play();
    }

    private void Poisoned()
    {
        _poisonSound.Play();
    }

    private void Healed()
    {
        _healSound.Play();
    }

    private void Death()
    {
        _deathSound.Play();
    }

    private void DestroyedPlank()
    {
        _destroyedPlankSound.Play();
    }

    private void StartEnemyTurn()
    {
        _nextTurnSound.Play();
    }
    
    private void StartPlayerTurn()
    {
        _beginTurnSound.Play();
    }
}
