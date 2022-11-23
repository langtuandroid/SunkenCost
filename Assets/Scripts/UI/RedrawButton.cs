using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedrawButton : InGameButton
{
    public static RedrawButton current;

    protected override void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;

        base.Awake();
    }

    private void Start()
    {
        GameEvents.current.OnPlayerBoughtStick += BoughtStick;
    }

    protected override bool TestForSuccess()
    {
        return GameManager.current.TryRedraw();
    }

    public override void CanClick(bool canClick)
    {
        base.CanClick(canClick && !PlayerController.current.IsOutOfMoves);
    }

    private void BoughtStick()
    {
        CanClick(true);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPlayerBoughtStick -= BoughtStick;
    }
}