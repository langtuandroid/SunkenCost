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
        
        public string Name { get; protected set; }
        
        public Stat MaxHealthStat { get; private set; }
        public int Gold { get; protected set; }
        public int Health { get; private set; }
        
        public int TurnOrder { get; private set; }
        public float Size { get; protected set; } = 1;

        public bool IsDestroyed { get; private set; } = false;

        public string Speech { get; private set; } = "";

        public EnemyMover Mover { get; } = new EnemyMover();

        public int MaxHealth => MaxHealthStat.Value;

        public Plank Plank => Board.Current.GetPlank(Mover.PlankNum);
        public int PlankNum => Mover.PlankNum;

        public bool FinishedMoving => Mover.FinishedMoving;

        protected virtual void Awake()
        {
            stats = new EnemyStats(this);

            Init();
            Mover.Init();
            ChangeHealth(MaxHealthStat.Value);
        }

        protected abstract void Init();

        protected void SetInitialHealth(int health)
        {
            // TODO: APPLY MODIFIERS
            MaxHealthStat = new Stat(health);
        }
        
        public List<BattleEvent> StartTurn()
        {
            var startTurnEvents = CreateEventAndResponses(BattleEventType.StartedIndividualEnemyTurn);
            
            // Apply poison
            if (stats.Poison > 0)
            {
                startTurnEvents.AddRange(DealPoison());
            }

            if (this is IStartOfTurnAbilityHolder startOfTurnAbilityHolder
                && startOfTurnAbilityHolder.GetIfUsingStartOfTurnAbility())
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
            
            if (PlankNum >= Board.Current.PlankCount)
            {
                var response = CreateEventAndResponses(BattleEventType.EnemyReachedBoat);
                response.AddRange(DestroySelf(DamageSource.Boat));
            }
            
            return CreateEventAndResponses(BattleEventType.EnemyMove);
        }

        public List<BattleEvent> EndTurn()
        {
            Mover.EndTurn();
            return CreateEventAndResponses(BattleEventType.EnemyEndMyMove);
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
        
        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.PlankMoved) RefreshMoverPlankNum();
            return false;
        }

        protected List<BattleEvent> Speak(string text)
        {
            Speech = text;
            var speechEventAndResponses = CreateEventAndResponses(BattleEventType.EnemySpeaking);
            Speech = "";

            return speechEventAndResponses;
        }

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
        
        private void RefreshMoverPlankNum()
        {
            if (Mover.PlankNum == -1) return;
            Mover.SetPlankNum(transform.parent.GetSiblingIndex());
        }

        public List<BattleEvent> DestroySelf(DamageSource damageSource)
        {
            IsDestroyed = true;
            
            return CreateEventAndResponses(BattleEventType.EnemyKilled, damageSource: damageSource);
        }
    }
}
