using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using TMPro;
using UnityEngine;

public class NextTurnButton : InGameButton
{
    public static NextTurnButton Current;
    private TextMeshProUGUI _textMeshProUGUI;

    protected override void Awake()
    {
        // One instance of static objects only
        if (Current)
        {
            Destroy(Current.gameObject);
        }
        
        Current = this;
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        base.Awake();
    }

    private void Start()
    {
        OldBattleEvents.Current.OnBeginPlayerTurn += UpdateText;
    }

    private void UpdateText()
    {
        if (Battle.Current.Turn == RunProgress.PlayerStats.NumberOfTurns + 1)
        {
            _textMeshProUGUI.text = "END BATTLE";
        }
    }

    protected override bool TryClick()
    {
        if (Battle.Current.GameState == GameState.PlayerTurn)
        {
            Battle.Current.NextTurn();
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        OldBattleEvents.Current.OnBeginPlayerTurn -= UpdateText;
    }
}
