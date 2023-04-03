using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public EnemyStats stats;
        
        public List<System.Action> PreMovingEffects = new List<System.Action>();
        
        private EnemyAnimationController _animationController;

        private int _turnOrder;
        
        public string Name { get; protected set; }
        
        public  Stat MaxHealth { get; private set; }
        protected int Gold { get; set; }
        public int Health { get; private set; }
        public float Size { get; protected set; } = 1;
        
        public bool Moving { get; private set; } = false;

        public bool IsDestroyed { get; private set; } = false;

        public EnemyMover Mover { get; private set; }
        public EnemyUI.EnemyUI UI { get; private set; }

        public Stick Stick => StickManager.current.GetStick(Mover.StickNum);
        public int StickNum => Mover.StickNum;

        public bool FinishedMoving => Mover.FinishedMoving;

        public int NextDirection => Mover.NextDirection;

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

        public IEnumerator ExecuteMoveStep()
        {
            Moving = true;

            if (Mover.FinishedMoving || !IsDestroyed)
            {

                // Change my stick
                Mover.Move();

                ActiveEnemiesManager.Current.EnemyMoved();

                yield return new WaitForSeconds(BattleManager.AttackTime / 16);

                // TEMPORARY Destroy if at end
                if (StickNum >= StickManager.current.stickCount)
                {
                    BattleEvents.Current.EnemyReachedEnd();
                    DestroySelf(true);
                    yield return 0;
                    yield break;
                }

                if (IsDestroyed) yield break;
            }

            Mover.UpdateMovementText();
            
            Moving = false;
        }

        public void BeginMyTurn()
        {
            _animationController.WiggleBeforeMoving();
        
            // Apply poison
            if (stats.Poison > 0)
            {
                PreMovingEffects.Add(Poison);
            }

            if (TestForStartOfTurnAbility())
            {
                PreMovingEffects.Add(StartOfTurnAbility);
            }
        
            UI.TurnOrderText.MyTurn();
        }
        
        public void BeginMyMove()
        {
        }

        public void EndMyTurn()
        {
            PreMovingEffects.Clear();
            UI.TurnOrderText.EndMyTurn();
        }

        private void Poison()
        {
            DamageHandler.DamageEnemy(stats.Poison, this, DamageSource.Poison);
            InGameSfxManager.current.Poisoned();
        }

        protected virtual void StartOfTurnAbility()
        {
        }

        protected virtual bool TestForStartOfTurnAbility()
        {
            return false;
        }

        protected virtual bool TestForPostMovingAbility()
        {
            return false;
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
            _animationController.Damage();
        }
    
        public void Heal(int amount)
        {
            var healAmount = amount;
            var healthDifference = (MaxHealth.Value - Health);
            if (healthDifference < healAmount) healAmount = healthDifference;
            
            ChangeHealth(healAmount);
            InGameSfxManager.current.Healed();
            _animationController.Heal();
            BattleEvents.Current.EnemyHealed(this);
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
            UI.HealthText.AlterHealth(Health, MaxHealth.Value);

            if (Health <= 0)
            {
                DestroySelf(true);
            }
        }

        public void SetTurnOrder(int turnOrder)
        {
            _turnOrder = turnOrder;
            UI.TurnOrderText.SetTurnOrder(turnOrder);
        }

        protected void Speak(string text)
        {
            UI.SpeechBubble.WriteText(text);
        }

        public abstract string GetDescription();

        public virtual void DestroySelf(bool killedByPlayer)
        {
            InGameSfxManager.current.Death();
            // TODO: CHANGE 
            if (killedByPlayer) BattleManager.Current.AlterGold(Gold);
            IsDestroyed = true;
            Moving = false;
            Log.current.AddEvent("E" + _turnOrder + " has been killed");
            ActiveEnemiesManager.Current.DestroyEnemy(this);
        }
    }
}
