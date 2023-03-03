using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhoseTurnText : MonoBehaviour
{
    public static WhoseTurnText current;

    [SerializeField] private TMP_ColorGradient yourColor;
    [SerializeField] private TMP_ColorGradient enemyColor;

    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        BattleEvents.Current.OnBeginPlayerTurn += UpdateText;
        BattleEvents.Current.OnBeginPlayerTurn += SetToPlayerColor;
        BattleEvents.Current.OnBeginEnemyTurn += SetToEnemyColor;
    }
    
    private void OnDestroy()
    {
        BattleEvents.Current.OnBeginPlayerTurn -= UpdateText;
        BattleEvents.Current.OnBeginPlayerTurn -= SetToPlayerColor;
        BattleEvents.Current.OnBeginEnemyTurn -= SetToEnemyColor;
    }

    public void SetToPlayerColor()
    {
        _textMeshProUGUI.colorGradientPreset = yourColor;
    }

    public void SetToEnemyColor()
    {
        _textMeshProUGUI.colorGradientPreset = enemyColor;
    }

    private void UpdateText()
    {
        var numberOfTurns = RunProgress.PlayerInventory.NumberOfTurns;
        var currentTurn = BattleManager.Current.Turn;
        
        if (currentTurn < numberOfTurns)
        {
            _textMeshProUGUI.text = "TURNS LEFT: " + (numberOfTurns + 1 - currentTurn);
        }
        else if (currentTurn == numberOfTurns)
        {
            _textMeshProUGUI.text = "LAST TURN!";
        }
        else
        {
            _textMeshProUGUI.text = "";
        }
    }
}
