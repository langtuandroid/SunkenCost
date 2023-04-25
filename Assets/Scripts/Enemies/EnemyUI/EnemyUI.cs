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
    public readonly struct EnemyUIDisplayState
    {
        public readonly BattleEventType battleEventType;
        public readonly string name;
        public readonly string description;
        public readonly int health;
        public readonly int maxHealth;
        public readonly int plankNum;
        public readonly int turnOrder;
        public readonly MovementType movementType;
        public readonly int moves;
        public readonly int poisonAmount;
        
        public readonly string speechText;

        public EnemyUIDisplayState(BattleEventType battleEventType, string name, string description, int health, int maxHealth, int plankNum, 
            int turnOrder, MovementType movementType, int moves, int poisonAmount, string speechText = "") =>
            (this.battleEventType, this.name, this.description, this.health, this.maxHealth, this.plankNum, this.turnOrder, this.movementType,
                this.moves, this.poisonAmount, this.speechText) =
            (battleEventType, name, description, health, maxHealth, plankNum, turnOrder, movementType, moves, poisonAmount, speechText);
    }
    
    public class EnemyUI : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private EnemyTransformPositioner _enemyTransformPositioner;

        [SerializeField] private TooltipTrigger _tooltipTrigger;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private EnemyMovementText _movementText;
        [SerializeField] private EnemyPoisonDisplay _poisonDisplay;
        [SerializeField] private EnemySpeechBubble _speechBubble;

        private Enemy _enemy;
        
        private Queue<EnemyUIDisplayState> _uiDisplayStates = new Queue<EnemyUIDisplayState>();

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            BattleEventsManager.Current.RegisterUIUpdater(this);
        }

        public bool GetIfUpdating(BattleEvent battleEvent)
        {
            return (battleEvent.enemyAffectee == _enemy || battleEvent.enemyAffector == _enemy);
        }

        public void SaveStateResponse(BattleEventType battleEventType)
        {
            var uiState = new EnemyUIDisplayState(battleEventType, _enemy.Name, _enemy.GetDescription(), _enemy.Health, 
                _enemy.MaxHealth, _enemy.PlankNum, _enemy.TurnOrder, _enemy.Mover.CurrentMove.movementType, 
                _enemy.Mover.AmountOfMovesLeftThisTurn, _enemy.stats.Poison, _enemy.Speech);

            _uiDisplayStates.Enqueue(uiState);
        }

        public void LoadNextState()
        {
            var state = _uiDisplayStates.Dequeue();
            UpdateUI(state);
        }
        
        private void UpdateUI(EnemyUIDisplayState state)
        {
            switch (state.battleEventType)
            {
                case BattleEventType.EnemyKilled:
                    Destroy(gameObject);
                    return;
                case BattleEventType.EnemyMove:
                    _enemyTransformPositioner.UpdatePosition(state.plankNum, state.turnOrder);
                    break;
                case BattleEventType.EnemySpeaking:
                    _speechBubble.DisplayText(state.speechText);
                    break;
            }

            _tooltipTrigger.header = state.name;
            _tooltipTrigger.content = state.description;
            _healthText.AlterHealth(state.health, state.maxHealth);
            _turnOrderText.SetTurnOrder(state.turnOrder);
            _movementText.UpdateMovementText(state.movementType, state.moves);
            _poisonDisplay.UpdateDisplay(state.poisonAmount);
        }
        
        private void OnDestroy()
        {
            BattleEventsManager.Current.DeregisterUIUpdater(this);
        }
    }
}