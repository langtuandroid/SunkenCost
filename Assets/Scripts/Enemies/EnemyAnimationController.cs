using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Animator _healAnimator;

    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();

        _healAnimator = transform.GetChild(8).GetChild(0).GetComponent<Animator>();

    }

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
