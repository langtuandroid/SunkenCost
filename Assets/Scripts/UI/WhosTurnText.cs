using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using TMPro;
using UnityEngine;

public class WhosTurnText : MonoBehaviour
{
    [SerializeField] private TMP_ColorGradient yourColor;
    [SerializeField] private TMP_ColorGradient enemyColor;

    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void StartEnemyTurn()
    {
        _textMeshProUGUI.colorGradientPreset = enemyColor;
    }

    public void EndEnemyTurn()
    {
        var turn = Battle.Current.Turn;
        var amountOfTurns = RunProgress.Current.PlayerStats.NumberOfTurns;
        
        if (turn < amountOfTurns)
        {
            _textMeshProUGUI.text = "TURNS LEFT: " + (amountOfTurns + 1 - turn);
        }
        else if (turn == amountOfTurns)
        {
            _textMeshProUGUI.text = "LAST TURN!";
        }
        else
        {
            _textMeshProUGUI.text = "";
        }
        
        _textMeshProUGUI.colorGradientPreset = yourColor;
    }

    public void EndBattle()
    {
        _textMeshProUGUI.colorGradientPreset = yourColor;
        _textMeshProUGUI.text = "Select reward!";
    }
}
