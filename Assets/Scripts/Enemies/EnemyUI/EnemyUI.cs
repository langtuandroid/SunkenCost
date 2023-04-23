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
        public readonly string title;
        public readonly string description;
        public readonly int health;
        public readonly int maxHealth;
        public readonly int turnOrder;
        public readonly MovementType movementType;
        public readonly int moves;
        
        public readonly string speechText;

        public EnemyUIDisplayState(string title, string description, int health, int maxHealth, int turnOrder,
            MovementType movementType, int moves, string speechText = "") =>
            (this.title, this.description, this.health, this.maxHealth, this.turnOrder, this.movementType, this.moves, this.speechText) =
            (title, description, health, maxHealth, turnOrder, movementType, moves, speechText);
    }
    
    public class EnemyUI : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private Image _image;
        [SerializeField] private EnemyPositioner _enemyPositioner;

        [SerializeField] private TooltipTrigger _tooltipTrigger;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private EnemyMovementText _movementText;
        
        [SerializeField] private Image _poisonImage;
        [SerializeField] private TextMeshProUGUI _poisonText;

        [SerializeField] private EnemySpeechBubble _speechBubble;

        private Enemy _enemy;
        
        private Queue<EnemyUIDisplayState> _uiDisplayStates = new Queue<EnemyUIDisplayState>();

        private string _speechText = "";
        
        public Image Image => _image;
        public TooltipTrigger TooltipTrigger => _tooltipTrigger;
        public EnemyTurnOrderText TurnOrderText => _turnOrderText;
        public EnemyHealthText HealthText => _healthText;

        public EnemyMovementText MovementText => _movementText;

        public Image PoisonImage => _poisonImage;
        public TextMeshProUGUI PoisonText => _poisonText;
        
        public EnemySpeechBubble SpeechBubble => _speechBubble;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            BattleEventsManager.Current.RegisterUIUpdater(this);
        }

        public bool GetIfUpdating(BattleEvent battleEvent)
        {
            return battleEvent.enemyAffectee == _enemy || battleEvent.enemyAffector == _enemy;
        }

        public void SaveCurrentState()
        {
            var uiState = new EnemyUIDisplayState(_enemy.Name, _enemy.GetDescription(), _enemy.Health, _enemy.MaxHealth, _enemy.TurnOrder, 
                _enemy.Mover.CurrentMove.movementType, _enemy.Mover.AmountOfMovesLeftThisTurn, _speechText);

            if (_speechText.Length > 0) _speechText = "";
            
            _uiDisplayStates.Enqueue(uiState);
        }

        public void LoadNextState()
        {
            var state = _uiDisplayStates.Dequeue();
            UpdateUI(state);
        }

        public void Speak(string text)
        {
            _speechText = text;
        }

        private void UpdateUI(EnemyUIDisplayState state)
        {
            _tooltipTrigger.header = state.title;
            _tooltipTrigger.content = state.description;
            
            _healthText.AlterHealth(state.health, state.maxHealth);
            
            _turnOrderText.SetTurnOrder(state.turnOrder);
            _movementText.UpdateMovementText(state.movementType, state.moves);
            
            if (state.speechText.Length > 0) SpeechBubble.DisplayText(state.speechText);
        }
        
        private void OnDestroy()
        {
            BattleEventsManager.Current.DeregisterUIUpdater(this);
        }
    }
}