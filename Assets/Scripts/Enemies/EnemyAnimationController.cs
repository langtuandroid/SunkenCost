using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _healAnimator;

    public void WiggleBeforeMoving()
    {
        _animator.Play("WiggleBeforeMoving");
    }

    public void Damage()
    {
        _animator.Play("Damaged");
    }

    public void Heal()
    {
        _healAnimator.Play("Heal");
    }
}
