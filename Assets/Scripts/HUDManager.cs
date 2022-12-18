using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;
    
    [SerializeField] private TextMeshProUGUI _movesText;
    [SerializeField] private TextMeshProUGUI _roundText;

    private List<Heart> _hearts = new List<Heart>();

    [SerializeField] private Image _xpFillBar;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private Transform heartsParentTransform;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }

        current = this;
    }

    private void Start()
    {
        BattleEvents.Current.OnBeginPlayerTurn += UpdateTurnText;
        
        for (var i = 0; i < heartsParentTransform.childCount; i++)
        {
            _hearts.Add(heartsParentTransform.GetChild(i).GetComponent<Heart>());
        }
        
        UpdateMovesText();
    }

    private void UpdateTurnText()
    {
        if (BattleManager.Current.Turn < BattleManager.NumberOfTurns)
        {
            _roundText.text = "TURNS LEFT: " + (BattleManager.NumberOfTurns + 1 - BattleManager.Current.Turn);
        }
        else if (BattleManager.Current.Turn == BattleManager.NumberOfTurns)
        {
            _roundText.text = "LAST TURN!";
        }
        else
        {
            _roundText.text = "EXTRACTION COMPLETE!";
        }
    }
    

    public void UpdateMovesText()
    {
        _movesText.text = (PlayerController.current.MovesPerTurn - PlayerController.current.MovesUsedThisTurn).ToString();
    }

    public void UpdateLives()
    {
        var lives = PlayerController.current.Lives;

        for (var i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].SetHeart(lives > i);
        }
    }

    private void OnDestroy()
    {
        BattleEvents.Current.OnBeginPlayerTurn -= UpdateTurnText;
    }
}
