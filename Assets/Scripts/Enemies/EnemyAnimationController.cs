using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour, IBattleEventUpdatedUI
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _healAnimator;

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
                return;
            case BattleEventType.EnemyHealed:
                Heal();
                return;
            case BattleEventType.StartedIndividualEnemyTurn:
                WiggleBeforeMoving();
                return;
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
}
