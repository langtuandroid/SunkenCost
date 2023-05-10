using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyUI : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private TooltipTrigger _tooltipTrigger;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private Image _healthSlider;
        
        [SerializeField] private EnemyMovementText _movementText;
        [SerializeField] private EnemyPoisonDisplay _poisonDisplay;
        [SerializeField] private EnemySpeechBubble _speechBubble;

        private Enemy _enemy;
        private EnemyMover _mover;
        

        private void Awake()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
            _enemy = GetComponent<Enemy>();
            _mover = _enemy.Mover;
        }

        private void Start()
        {
            _tooltipTrigger.header = _enemy.Name;
            Invoke(nameof(UpdateUI), 0.01f);
        }

        private void OnDestroy()
        {
            if (BattleRenderer.Current)
                BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type != BattleEventType.StartNextPlayerTurn
                && battleEvent.affectedResponderID != _enemy.ResponderID 
                && battleEvent.affectingResponderID != _enemy.ResponderID) return;

            if (battleEvent.type == BattleEventType.EnemyReachedBoat)
            {
                _turnOrderText.gameObject.SetActive(false);
                _movementText.gameObject.SetActive(false);
                _healthText.gameObject.SetActive(false);
                _poisonDisplay.gameObject.SetActive(false);
                return;
            }
            
            if (_enemy.IsDestroyed) return;
            
            if (battleEvent.type == BattleEventType.EnemySpeaking) _speechBubble.DisplayText(_enemy.Speech);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _tooltipTrigger.content = _enemy.GetDescription();
            _turnOrderText.SetTurnOrder(_enemy.TurnOrder);
            _movementText.UpdateMovementText(_mover.CurrentMove.MovementType, _mover.AmountOfMovesLeftThisTurn);
            _healthText.AlterHealth(_enemy.Health, _enemy.MaxHealth);
            _healthSlider.fillAmount = (float)_enemy.Health / _enemy.MaxHealth;
            _poisonDisplay.UpdateDisplay(_enemy.stats.Poison);
        }
    }
}