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

    public void UpdateText()
    {
        _textMeshProUGUI.text = "END BATTLE";
    }

    protected override bool TryClick()
    {
        if (Battle.Current.GameState == GameState.PlayerTurn)
        {
            Battle.Current.ClickedNextTurn();
            return true;
        }
        return false;
    }
}
