using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyUI : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private CanvasGroup _visualsGroup;
        
        [SerializeField] private TooltipTrigger _tooltipTrigger;

        [SerializeField] private EnemyAnimator _enemyAnimator;
        
        [SerializeField] private EnemyTurnOrderText _turnOrderText;
        [SerializeField] private EnemyHealthText _healthText;
        [SerializeField] private Image _healthSlider;
        [SerializeField] private DamageVisualiser _damageVisualiser;
        
        [SerializeField] private EnemyMovementText _movementText;
        [SerializeField] private EnemyPoisonDisplay _poisonDisplay;
        [SerializeField] private EnemySpeechBubble _speechBubble;

        private Enemy _enemy;
        private EnemyMover _mover;

        private void Awake()
        {
            _visualsGroup.alpha = 0f;
        }

        private void Start()
        {
            StartCoroutine(Init());
            
            BattleRenderer.Current.RegisterUIUpdater(this);
            _enemy = transform.parent.GetComponent<Enemy>();
            _enemyAnimator.Init(_enemy.Asset.SpritePack);
            _mover = _enemy.Mover;
            
            _tooltipTrigger.header = _enemy.Name;
        }

        private IEnumerator Init()
        {
            yield return 0;
            _visualsGroup.alpha = 1f;
            _visualsGroup.enabled = false;
            UpdateUI();
        }

        private void OnDestroy()
        {
            if (BattleRenderer.Current)
                BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EnemySpawned || 
                (battleEvent.type != BattleEventType.StartedNextPlayerTurn
                 && battleEvent.primaryResponderID != _enemy.ResponderID
                 && battleEvent.secondaryResponderID != _enemy.ResponderID)) return;

            if (_enemy.IsDestroyed) return;
            
            if (battleEvent.type == BattleEventType.EnemySpeaking) _speechBubble.DisplayText(_enemy.Speech);
            
            if (battleEvent.type == BattleEventType.EnemyAttacked) 
                _damageVisualiser.Damage(battleEvent.modifier, battleEvent.damageModificationPackage);
            else if (battleEvent.type == BattleEventType.EnemyHealed)
                _damageVisualiser.Heal(battleEvent.modifier);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _tooltipTrigger.content = _enemy.GetDescription();
            _turnOrderText.SetTurnOrder(_enemy.TurnOrder);
            _movementText.UpdateMovementText(_mover.CurrentMove.MovementType, _mover.MovementRemainingThisTurn);
            _healthText.AlterHealth(_enemy.Health, _enemy.MaxHealth);
            _healthSlider.fillAmount = (float)_enemy.Health / _enemy.MaxHealth;
            _poisonDisplay.UpdateDisplay(_enemy.stats.Poison);
        }
    }
}