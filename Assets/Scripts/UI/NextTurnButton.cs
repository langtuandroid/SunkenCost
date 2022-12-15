using System;
using System.Collections;
using System.Collections.Generic;
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
        BattleEvents.Current.OnBeginPlayerTurn += UpdateText;
    }

    protected override bool TestForSuccess()
    {
        return BattleManager.Current.TryNextTurn();
    }

    private void UpdateText()
    {
        if (BattleManager.Current.Turn == BattleManager.NumberOfTurns + 1)
        {
            _textMeshProUGUI.text = "END BATTLE";
        }
    }

    private void OnDestroy()
    {
        BattleEvents.Current.OnBeginPlayerTurn -= UpdateText;
    }
}
