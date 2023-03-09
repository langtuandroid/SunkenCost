using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleHUDManager : MonoBehaviour
{
    public static BattleHUDManager current;

    [SerializeField] private Hearts hearts;
    
    [SerializeField] private TextMeshProUGUI _movesText;

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
        BattleEvents.Current.OnBeginPlayerTurn += UpdateMovesText;
        BattleEvents.Current.OnLostLife += UpdateLives;
        
        UpdateLives();
        UpdateMovesText();
    }


    public void UpdateMovesText()
    {
        var movesLeft = (PlayerController.current.MovesPerTurn - PlayerController.current.MovesUsedThisTurn).ToString();
        _movesText.text = movesLeft + "/" + PlayerController.current.MovesPerTurn;
    }

    private void UpdateLives()
    {
        hearts.UpdateLives(PlayerController.current.Lives);
    }
}
