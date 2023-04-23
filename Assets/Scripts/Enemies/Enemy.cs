using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies.EnemyUI;
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
        
        public Stat MaxHealthStat { get; private set; }
        public int Gold { get; protected set; }
        public int Health { get; private set; }
        
        public int TurnOrder { get; private set; }
        public float Size { get; protected set; } = 1;

        public bool Moving { get; private set; } = false;

        public bool IsDestroyed { get; private set; } = false;

        public EnemyMover Mover { get; private set; }
        public EnemyUI.EnemyUI UI { get; private set; }

        public int MaxHealth => MaxHealthStat.Value;

        public Plank Plank => Board.Current.GetPlank(Mover.StickNum);
        public int PlankNum => Mover.StickNum;

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
            ChangeHealth(MaxHealthStat.Value);
        }

        protected void SetInitialHealth(int health)
        {
            // TODO: APPLY MODIFIERS
            MaxHealthStat = new Stat(health);
        }
        
        public List<BattleEvent> StartTurn()
        {
            var startTurnEvents = new List<BattleEvent>();
            
            var startTurnEvent = CreateEventAndResponses(BattleEventType.StartedIndividualEnemyTurn);
            
            // Apply poison
            if (stats.Poison > 0)
            {
                startTurnEvents.AddRange(DealPoison());
            }

            if (this is IStartOfTurnAbilityHolder startOfTurnAbilityHolder)
            {
                foreach (var battleEvent in startOfTurnAbilityHolder.GetStartOfTurnAbility())
                {
                    startTurnEvents.AddRange(BattleEventsManager.Current.GetEventAndResponsesList(battleEvent));
                }
            }

            return startTurnEvents;
        }

        public List<BattleEvent> MoveStep()
        {
            // Change my stick
            Mover.Move();

            // TEMPORARY Destroy if at end
            if (PlankNum >= Board.Current.PlankCount)
            {
                DestroySelf(DamageSource.Boat);
                
                return CreateEventAndResponses(BattleEventType.EnemyReachedBoat);
            }
            
            return CreateEventAndResponses(BattleEventType.EnemyMove);
        }

        public List<BattleEvent> EndTurn()
        {
            Mover.EndTurn();
            return CreateEventAndResponses(BattleEventType.EnemyEndMyMove);
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
            UI.HealthText.AlterHealth(Health, MaxHealthStat.Value);
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

        public BattleEvent Block(int blockAmount)
        {
            Mover.Block(blockAmount);
            return CreateEvent(BattleEventType.EnemyBlocked, blockAmount);
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
    
        public BattleEvent Heal(int amount)
        {
            var healAmount = amount;
            var healthDifference = (MaxHealthStat.Value - Health);
            if (healthDifference < healAmount) healAmount = healthDifference;
            
            ChangeHealth(healAmount);

            return CreateEvent(BattleEventType.EnemyHealed, healAmount);
        }

        public void AddMaxHealthModifier(StatModifier statModifier)
        {
            MaxHealthStat.AddModifier(statModifier);
            var overHeal = Health - MaxHealthStat.Value;
            
            ChangeHealth(overHeal > 0 ? -overHeal : 0);
        }

        public void RemoveMaxHealthModifier(StatModifier statModifier)
        {
            MaxHealthStat.RemoveModifier(statModifier);
            ChangeHealth(0);
        }

        private void ChangeHealth(int amount)
        {
            Health += amount;
        }

        public void SetTurnOrder(int turnOrder)
        {
            TurnOrder = turnOrder;
        }

        public abstract string GetDescription();

        protected BattleEvent CreateEvent(BattleEventType type, int modifier = 0,
            DamageSource damageSource = DamageSource.None)
        {
            return new BattleEvent(type) 
                {enemyAffectee = this, modifier = modifier, damageSource = damageSource};
        }

        private List<BattleEvent> CreateEventAndResponses(BattleEventType type, int modifier = 0, 
            DamageSource damageSource = DamageSource.None)
        {
            var battleEvent = CreateEvent(type, modifier, damageSource);
            return BattleEventsManager.Current.GetEventAndResponsesList(battleEvent);
        }

        public virtual List<BattleEvent> DestroySelf(DamageSource damageSource)
        {
            IsDestroyed = true;
            Moving = false;
            
            return CreateEventAndResponses(BattleEventType.EnemyKilled, damageSource: damageSource);
        }

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return false;
        }
    }
}
