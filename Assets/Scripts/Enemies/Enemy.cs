using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;
using BattleScreen.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class Enemy : BattleEventResponder
    {
        public EnemyStats stats;

        private EnemyAnimationController _animationController;
        
        
        public string Name { get; protected set; }
        
        public  Stat MaxHealth { get; private set; }
        public int Gold { get; protected set; }
        public int Health { get; private set; }
        public float Size { get; protected set; } = 1;
        
        public bool Moving { get; private set; } = false;

        public bool IsDestroyed { get; private set; } = false;

        public EnemyMover Mover { get; private set; }
        public EnemyUI.EnemyUI UI { get; private set; }

        public Plank Plank => PlankMap.Current.GetPlank(Mover.StickNum);
        public int StickNum => Mover.StickNum;

        public bool FinishedMoving => Mover.FinishedMoving;

        protected virtual void Awake()
        {
            UI = GetComponent<EnemyUI.EnemyUI>();
            Mover = GetComponent<EnemyMover>();

            stats = new EnemyStats(this);
            _animationController = GetComponent<EnemyAnimationController>();

            Init();
        }

        protected abstract void Init();
        
        protected virtual void Start()
        {
            ChangeHealth(MaxHealth.Value);
            UI.TooltipTrigger.header = Name;
        }

        protected void SetInitialHealth(int health)
        {
            // TODO: APPLY MODIFIERS
            MaxHealth = new Stat(health);
        }
        
        public List<BattleEvent> StartTurn()
        {
            var startTurnEvents = new List<BattleEvent>();
            
            var startTurnEvent = new EnemyBattleEvent(BattleEventType.StartedIndividualEnemyTurn, this);
            startTurnEvents.AddRange(
                BattleEventsManager.Current.GetEventAndResponsesList(startTurnEvent));
            
            // Apply poison
            if (stats.Poison > 0)
            {
                startTurnEvents.AddRange(DealPoison());
            }

            if (this is IStartOfTurnAbilityHolder startOfTurnAbilityHolder)
            {
                startTurnEvents.AddRange(startOfTurnAbilityHolder.GetStartOfTurnAbility());
            }

            return startTurnEvents;
        }

        public List<BattleEvent> MoveStep()
        {
            // Change my stick
            Mover.Move();

            // TEMPORARY Destroy if at end
            if (StickNum >= PlankMap.Current.PlankCount)
            {
                DestroySelf(DamageSource.Boat);
                
                var enemyReachedBoatEvent = new EnemyBattleEvent(BattleEventType.EnemyReachedBoat, this);
                return (BattleEventsManager.Current.GetEventAndResponsesList(enemyReachedBoatEvent));
            }
            
            var enemyMovedStepTurnAction = new EnemyBattleEvent(BattleEventType.EnemyMove, this);
            
            return
                BattleEventsManager.Current.GetEventAndResponsesList(enemyMovedStepTurnAction);
        }

        public List<BattleEvent> EndTurn()
        {
            var endTurnEvent = new EnemyBattleEvent(BattleEventType.EnemyEndTurn, this);
            return BattleEventsManager.Current.GetEventAndResponsesList(endTurnEvent);
        }

        public IEnumerator MoveAnimation()
        {
            Mover.UpdateMovementText();
            
            yield break;
        }

        public IEnumerator BeginMyTurnAnimation()
        {
            _animationController.WiggleBeforeMoving();
            UI.TurnOrderText.MyTurn();
            
            yield break;
        }

        public IEnumerator HealAnimation()
        {
            InGameSfxManager.current.Healed();
            UI.HealthText.AlterHealth(Health, MaxHealth.Value);
            _animationController.Heal();
            
            yield break;
        }

        public IEnumerator DamageAnimation()
        {
            _animationController.Damage();
            yield break;
        }

        public IEnumerator EndMyTurnAnimation()
        {
            UI.TurnOrderText.EndMyTurn();
            yield break;
        }

        private List<BattleEvent> DealPoison()
        {
            return BattleEventsManager.Current.GetEventAndResponsesList(DamageHandler.DamageEnemy
                (stats.Poison, this, DamageSource.Poison));
            
        }

        public void Block(int blockAmount)
        {
            Mover.Block(blockAmount);
        }

        public void RePosition(Vector2 newPosition)
        {
            Mover.SetAimPosition(newPosition);
        }

        public virtual void TakeDamage(int damage, DamageSource damageSource)
        {
            ChangeHealth(-damage);
            
            if (Health <= 0)
            {
                DestroySelf(damageSource);
            }
        }
    
        public List<BattleEvent> Heal(int amount)
        {
            var healAmount = amount;
            var healthDifference = (MaxHealth.Value - Health);
            if (healthDifference < healAmount) healAmount = healthDifference;
            
            ChangeHealth(healAmount);

            var healingEvent = new EnemyHealBattleEvent(this, healAmount);
            return BattleEventsManager.Current.GetEventAndResponsesList(healingEvent);
        }

        public void AddMaxHealthModifier(StatModifier statModifier)
        {
            MaxHealth.AddModifier(statModifier);
            var overHeal = Health - MaxHealth.Value;
            
            ChangeHealth(overHeal > 0 ? -overHeal : 0);
        }

        public void RemoveMaxHealthModifier(StatModifier statModifier)
        {
            MaxHealth.RemoveModifier(statModifier);
            ChangeHealth(0);
        }

        private void ChangeHealth(int amount)
        {
            Health += amount;
        }

        public void SetTurnOrder(int turnOrder)
        {
            UI.TurnOrderText.SetTurnOrder(turnOrder);
        }

        protected void Speak(string text)
        {
            UI.SpeechBubble.WriteText(text);
        }

        public abstract string GetDescription();

        public virtual List<BattleEvent> DestroySelf(DamageSource damageSource)
        {
            IsDestroyed = true;
            Moving = false;
            
            var enemyKilledEvent = new EnemyKillBattleEvent(this, damageSource);
            return BattleEventsManager.Current.GetEventAndResponsesList(enemyKilledEvent);
        }
    }
}
