using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using FMOD.Studio;
using FMODUnity;

public class InGameSfxManager : MonoBehaviour
{
    public static InGameSfxManager current;

    private AudioSource _audioSource;

    [SerializeField] private StudioEventEmitter _successfulBuyPlankSound;
    [SerializeField] private StudioEventEmitter _goodClickSound;
    [SerializeField] private StudioEventEmitter _badClickSound;
    [SerializeField] private StudioEventEmitter _stickMovementSound;
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
    
    private void OfferedDesigns()
    {
        _successfulBuyPlankSound.Play();
    }

    private void MovedStick()
    {
        _stickMovementSound.Play();
    }

    public void GoodClick()
    {
        _goodClickSound.Play();
    }

    public void BadClick()
    {
        _badClickSound.Play();
    }

    public void EnemyMoved()
    {
        _charMovementSound.Play();
    }

    public void DamagedEnemy()
    {
        _damageSound.Play();
    }

    public void Slimed()
    {
        _slimedSound.Play();
    }

    public void Poisoned()
    {
        _poisonSound.Play();
    }

    public void Healed()
    {
        _healSound.Play();
    }

    public void Death()
    {
        _deathSound.Play();
    }

    public void DestroyedPlank()
    {
        _destroyedPlankSound.Play();
    }

    public void NextTurn()
    {
        _nextTurnSound.Play();
    }
    
    public void BeginTurn()
    {
        StartCoroutine(WaitToBeginTurn());
    }

    private IEnumerator WaitToBeginTurn()
    {
        yield return new WaitForSeconds(0.2f);
        _beginTurnSound.Play();
    }
}
