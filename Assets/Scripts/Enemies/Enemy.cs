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
        
        public EnemyAsset Asset { get; private set; }
        
        public string Name { get; protected set; }
        
        public Stat MaxHealthStat { get; private set; }
        public int Gold { get; protected set; } = 1;
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
            Asset = EnemyLoader.EnemyAssets.First(a => a.Class == GetType());
            stats = new EnemyStats(ResponderID, Asset.Modifiers);
            Name = Asset.Name;
            SetInitialHealth(Asset.MaxHealth);
            Mover.Init(Asset.Moves);
        }

        protected virtual void SetInitialHealth(int health)
        {
            MaxHealthStat = new Stat(health);
            Health = MaxHealthStat.Value;
        }
        
        public virtual int GetBoatDamage()
        {
            return Health;
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

        public BattleEvent AddMaxHealthModifier(StatModifier statModifier)
        {
            MaxHealthStat.AddModifier(statModifier);
            var overHeal = Health - MaxHealth;
            
            ChangeHealth(overHeal > 0 ? -overHeal : 0);

            var maxHealthModifiedEvent = CreateEvent(BattleEventType.EnemyMaxHealthModified);
            maxHealthModifiedEvent.modifier = (int)statModifier.Value;
            return maxHealthModifiedEvent;
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
            switch (previousBattleEvent.type)
            {
                case BattleEventType.EnemySpawned when previousBattleEvent.primaryResponderID == ResponderID
                                                       && this is ISpawnAbilityHolder abilityHolder:
                {
                    return abilityHolder.GetSpawnAbility();
                }

                // Damaged
                case BattleEventType.EnemyAttacked when previousBattleEvent.primaryResponderID == ResponderID:
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
                
                case BattleEventType.EndedEnemyTurn:
                    Mover.EndTurn();
                    break;
            }

            return BattleEventPackage.Empty;
        }
        
        public BattleEventPackage Die(DamageSource source)
        {
            IsDestroyed = true;
            
            var eventList = new List<BattleEvent>();
            
            eventList.Add(CreateEvent(BattleEventType.EnemyKilled, damageSource: source));
            eventList.Add(CreateEvent(BattleEventType.TryGainedGold, Gold, source));
            if (IsMyTurn) eventList.Add(CreateEvent(BattleEventType.EndedIndividualEnemyTurn));
            return new BattleEventPackage(eventList);
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
                {primaryResponderID = ResponderID, modifier = modifier, source = damageSource};
        }

        private void ChangeHealth(int amount)
        {
            Health += amount;
        }
    }
}
