using System;
using System.Collections;
using System.Collections.Generic;
using OfferScreen;
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

    [SerializeField] private GoldDisplay _goldDisplay;

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
        OldBattleEvents.Current.OnBeginPlayerTurn += UpdateMovesText;
        OldBattleEvents.Current.OnPlayerLostLife += UpdateLives;
        OldBattleEvents.Current.OnPlayerGainedGold += UpdateGoldText;

        UpdateLives();
        UpdateMovesText();
    }


    public void UpdateMovesText()
    {
        var movesLeft = (Player.Current.MovesPerTurn - Player.Current.MovesUsedThisTurn).ToString();
        _movesText.text = movesLeft + "/" + Player.Current.MovesPerTurn;
    }

    private void UpdateLives()
    {
        hearts.UpdateLives(Player.Current.Lives);
    }

    private void UpdateGoldText()
    {
        _goldDisplay.UpdateText(RunProgress.PlayerStats.Gold);
    }
}
