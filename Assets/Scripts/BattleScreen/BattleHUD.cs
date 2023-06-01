using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using OfferScreen;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour, IBattleEventUpdatedUI
{
    [SerializeField] private BoatShaker _boatShaker;
    [SerializeField] private BoatHealthMeter _boatHealthMeter;
    
    [SerializeField] private TextMeshProUGUI _movesText;

    [SerializeField] private GoldDisplay _goldDisplay;

    [SerializeField] private NextTurnButton _nextTurnButton;
    [SerializeField] private WhosTurnText _whosTurnText;

    private void Start()
    {
        BattleRenderer.Current.RegisterUIUpdater(this);
        
        UpdateMovesText();
        UpdateGoldText();
        _whosTurnText.EndEnemyTurn();
    }

    public void UpdateMovesText()
    {
        var movesLeft = (Player.Current.MovesPerTurn - Player.Current.MovesUsedThisTurn).ToString();
        _movesText.text = movesLeft + "/" + Player.Current.MovesPerTurn;
    }

    private void EndedBattle()
    {
    }

    private void GainedLife()
    {
        UpdateLives();
    }

    private void LostLife()
    {
        UpdateLives();
    }

    private void UpdateLives()
    {
        _boatShaker.Shake();
        _boatHealthMeter.RefreshMeter(Player.Current.Health, RunProgress.Current.PlayerStats.MaxHealth);
    }

    private void UpdateGoldText()
    {
        _goldDisplay.UpdateText(Player.Current.Gold);
    }

    private void StartEnemyTurn()
    {
        _whosTurnText.StartEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        _whosTurnText.EndEnemyTurn();
        UpdateMovesText();
    }

    private void PlankAddedOrRemoved()
    {
        BoardScaler.current.SetBoardScale(Board.Current.PlankCount);
    }

    public void RespondToBattleEvent(BattleEvent battleEvent)
    {
        switch (battleEvent.type)
        {
            case BattleEventType.GainedGold:
                UpdateGoldText();
                break;
            case BattleEventType.PlayerGainedLife:
                GainedLife();
                break;
            case BattleEventType.PlayerLostLife:
                LostLife();
                break;
            case BattleEventType.PlayerUsedMove:
                UpdateMovesText();
                break;
            case BattleEventType.EndedBattle:
                EndedBattle();
                break;
            case BattleEventType.StartedEnemyTurn:
                StartEnemyTurn();
                break;
            case BattleEventType.StartedNextPlayerTurn:
                EndEnemyTurn();
                break;
            case BattleEventType.PlankCreated: case BattleEventType.PlankDestroyed:
                PlankAddedOrRemoved();
                break;
        }
    }
}
