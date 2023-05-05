using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;
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
        public bool IsMyTurn { get; set; }
        public float Size { get; protected set; } = 1;

        public bool IsDestroyed { get; private set; } = false;

        public string Speech { get; private set; } = "";

        public EnemyMover Mover { get; } = new EnemyMover();

        public int MaxHealth => MaxHealthStat.Value;

        public Plank Plank => Board.Current.GetPlank(Mover.PlankNum);
        public int PlankNum => Mover.PlankNum;

        public bool FinishedMoving => Mover.FinishedMoving;

        protected override void Awake()
        {
            base.Awake();
            stats = new EnemyStats(ResponderID);

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

        public BattleEvent DealPoisonDamage()
        {
            return DamageHandler.DamageEnemy
                (stats.Poison, ResponderID, DamageSource.Poison);
        }

        public BattleEvent Block(int blockAmount)
        {
            Mover.Block(blockAmount);
            return CreateEvent(BattleEventType.EnemyBlocked, blockAmount);
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
        
        public void RefreshPlankNum()
        {
            if (Mover.PlankNum == -1) return;
            Mover.SetPlankNum(transform.parent.GetSiblingIndex());
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            // Damaged
            if (previousBattleEvent.type == BattleEventType.EnemyAttacked 
                && previousBattleEvent.affectedResponderID == ResponderID)
            {
                ChangeHealth(-previousBattleEvent.modifier);
                
                // Killed 
                if (Health <= 0)
                {
                    return Die(previousBattleEvent.source);
                }

                var damagedEvent = CreateEvent
                    (BattleEventType.EnemyDamaged, previousBattleEvent.modifier, previousBattleEvent.source);
                return new BattleEventPackage(damagedEvent);
            }

            if (previousBattleEvent.type == BattleEventType.EndedEnemyTurn)
            {
                Mover.EndTurn();
            }
            
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
            return new BattleEvent(type) 
                {affectedResponderID = ResponderID, modifier = modifier, source = damageSource};
        }

        private void ChangeHealth(int amount)
        {
            Health += amount;
        }

        public BattleEventPackage Die(DamageSource source)
        {
            var eventList = new List<BattleEvent>();
            
            if (source == DamageSource.Boat) eventList.Add(CreateEvent(BattleEventType.EnemyReachedBoat));

            eventList.Add(CreateEvent(BattleEventType.EnemyKilled, damageSource: source));
            eventList.Add(CreateEvent(BattleEventType.TryGainedGold, Gold, source));

            if (IsMyTurn) eventList.Add(CreateEvent(BattleEventType.EndedIndividualEnemyTurn));
            
            IsDestroyed = true;
            return new BattleEventPackage(eventList);
        }
    }
}
