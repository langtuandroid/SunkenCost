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

    private void Awake()
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
        if (battleEvent.affectedResponderID != _responderID) return;

        switch (battleEvent.type)
        {
            case BattleEventType.EnemyDamaged:
                Damage();
                break;
            case BattleEventType.EnemyHealed:
                Heal();
                break;
            case BattleEventType.StartedIndividualEnemyTurn:
                WiggleBeforeMoving();
                break;
            case BattleEventType.EnemyReachedBoat:
                Die();
                break;
            case BattleEventType.EnemyKilled when battleEvent.source != DamageSource.Boat:
                Die();
                break;
        }
    }
    
    private void WiggleBeforeMoving()
    {
        _animator.Play("WiggleBeforeMoving");
    }

    private void Damage()
    {
        _animator.Play("Damaged");
    }

    private void Heal()
    {
        _healAnimator.Play("Heal");
    }

    private void Die()
    {
        _shaderAnimation.StartDeathAnimation();
    }
}
