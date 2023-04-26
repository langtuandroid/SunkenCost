using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        private bool _yetToApplyPoisonThisTurn;
        private bool _yetToExecuteStartOfTurnAbilityThisTurn;
        private bool _yetToExecuteEndOfTurnAbilityThisTurn;
        
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

        public void CalculateActionsForTurn()
        {
            _yetToApplyPoisonThisTurn = stats.Poison > 0;
            _yetToExecuteStartOfTurnAbilityThisTurn = this is IStartOfTurnAbilityHolder holder && holder.GetIfUsingStartOfTurnAbility();
            _yetToExecuteEndOfTurnAbilityThisTurn = false;
        }
        
        public BattleEventPackage GetNextAction()
        {
            if (_yetToApplyPoisonThisTurn)
            {
                _yetToApplyPoisonThisTurn = false;
                return new BattleEventPackage(DealPoisonDamage());
            }

            if (_yetToExecuteStartOfTurnAbilityThisTurn)
            {
                if (this is IStartOfTurnAbilityHolder startOfTurnAbilityHolder)
                {
                    _yetToExecuteStartOfTurnAbilityThisTurn = false;
                    return startOfTurnAbilityHolder.GetStartOfTurnAbility();
                }
                
                throw new Exception("Should not be executing a start of turn ability when this is not capable!");
            }

            if (!FinishedMoving)
            {
                return MoveStep();
            }

            if (_yetToExecuteEndOfTurnAbilityThisTurn)
            {
                throw new NotImplementedException();
            }

            Mover.EndTurn();
            return new BattleEventPackage(CreateEvent(BattleEventType.EndedIndivdualEnemyMove));
        }

        private BattleEventPackage MoveStep()
        {
            // Change my stick
            Mover.Move();
            
            return PlankNum >= Board.Current.PlankCount 
                ? new BattleEventPackage(CreateEvent(BattleEventType.EnemyReachedBoat), DestroySelf(DamageSource.Boat)) 
                : new BattleEventPackage(CreateEvent(BattleEventType.EnemyMove));
        }

        private BattleEvent DealPoisonDamage()
        {
            return DamageHandler.DamageEnemy
                (stats.Poison, this, DamageSource.Poison);
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

        public void SetTurnOrder(int turnOrder)
        {
            TurnOrder = turnOrder;
        }

        public abstract string GetDescription();

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            if (previousBattleEvent.type == BattleEventType.PlankMoved) RefreshMoverPlankNum();
            return BattleEventPackage.Empty;
        }

        protected BattleEvent Speak(string text)
        {
            Speech = text;
            return CreateEvent(BattleEventType.EnemySpeaking);
        }

        protected BattleEvent CreateEvent(BattleEventType type, int modifier = 0,
            DamageSource damageSource = DamageSource.None)
        {
            return new BattleEvent(type, this) 
                {enemyAffectee = this, modifier = modifier, damageSource = damageSource};
        }
        
        private void ChangeHealth(int amount)
        {
            Health += amount;
        }

        private void RefreshMoverPlankNum()
        {
            if (Mover.PlankNum == -1) return;
            Mover.SetPlankNum(transform.parent.GetSiblingIndex());
        }

        private BattleEvent DestroySelf(DamageSource damageSource)
        {
            IsDestroyed = true;
            return CreateEvent(BattleEventType.EnemyKilled, damageSource: damageSource);
        }
    }
}
