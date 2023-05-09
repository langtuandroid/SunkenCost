using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using TMPro;
using UnityEngine;

public class NextTurnButton : InGameButton
{
    private TextMeshProUGUI _textMeshProUGUI;

    protected override void Awake()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        base.Awake();
    }
    protected override bool TryClick()
    {
        if (Battle.Current.GameState == GameState.PlayerActionPeriod)
        {
            Battle.Current.ClickedNextTurn();
            return true;
        }
        return false;
    }
}
