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
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private TooltipTrigger _tooltipTrigger;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private EnemyMovementText _movementText;
        [SerializeField] private EnemyPoisonDisplay _poisonDisplay;
        [SerializeField] private EnemySpeechBubble _speechBubble;

        private Enemy _enemy;
        private EnemyMover _mover;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _mover = _enemy.Mover;
            BattleRenderer.Current.RegisterUIUpdater(this);
        }

        private void OnDestroy()
        {
            if (BattleRenderer.Current)
                BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.enemyAffectee != _enemy && battleEvent.enemyAffector != _enemy) return;
            
            switch (battleEvent.type)
            {
                case BattleEventType.EnemyKilled:
                    return;
                case BattleEventType.EnemySpeaking:
                    _speechBubble.DisplayText(_enemy.Speech);
                    break;
            }
            
            _tooltipTrigger.header = _enemy.Name;
            _tooltipTrigger.content = _enemy.GetDescription();
            _healthText.AlterHealth(_enemy.Health, _enemy.MaxHealth);
            _turnOrderText.SetTurnOrder(_enemy.TurnOrder);
            _movementText.UpdateMovementText(_mover.CurrentMove.movementType, _mover.AmountOfMovesLeftThisTurn);
            _poisonDisplay.UpdateDisplay(_enemy.stats.Poison);
            
            Debug.Log(battleEvent.type + " " + _enemy.Health);
        }
    }
}