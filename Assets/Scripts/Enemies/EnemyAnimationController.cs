using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Enemies.EnemyUI;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAnimationController : MonoBehaviour, IBattleEventUpdatedUI
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _healAnimator;
    [FormerlySerializedAs("_destroyAnimation")] [SerializeField] private EnemyShaderAnimation _shaderAnimation;

    private int _responderID;
    
    private static float AnimationSpeed => 0.5f + (1f - Battle.ActionExecutionSpeed);

    private void Start()
    {
        BattleRenderer.Current.RegisterUIUpdater(this);
        _responderID = GetComponent<Enemy>().ResponderID;
    }

    private void OnDestroy()
    {
        if (BattleRenderer.Current)
            BattleRenderer.Current.DeregisterUIUpdater(this);
    }

    public void RespondToBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent.primaryResponderID != _responderID) return;

        switch (battleEvent.type)
        {
            case BattleEventType.EnemyAttacked: case BattleEventType.EnemyMaxHealthModified when battleEvent.modifier < 0:
                Damage();
                break;
            case BattleEventType.EnemyHealed: case BattleEventType.EnemyMaxHealthModified when battleEvent.modifier > 0:
                Heal();
                break;
            case BattleEventType.StartedIndividualEnemyTurn:
                WiggleBeforeMoving();
                break;
            /*
            case BattleEventType.EnemyReachedBoat:
                Die();
                break;
                */
            case BattleEventType.EnemyKilled when battleEvent.source != DamageSource.Boat:
                Die();
                break;
        }
    }

    private void Play(string stateName)
    {
        _animator.speed = AnimationSpeed;
        _animator.Play(stateName);
    }
    
    private void WiggleBeforeMoving()
    {
        Play("WiggleBeforeMoving");
    }

    private void Damage()
    {
        Play("Damaged");
    }

    private void Heal()
    {
        _healAnimator.speed = AnimationSpeed;
        _healAnimator.Play("Heal");
    }

    private void Die()
    {
        _shaderAnimation.StartDeathAnimation();
    }
}
