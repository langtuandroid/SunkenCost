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
        public const int EnemyOffset = 100;

        private TooltipTrigger _tooltipTrigger;
        public string Name { get; protected set; }

        public int MoveMin { get; protected set; }
        public int MoveMax { get; protected set; }
        public  Stat MaxHealth { get; private set; }
        protected int Gold { get; set; }
        protected int Health { get; private set; }
        public float Size { get; protected set; } = 1;

        protected List<int> MoveSet = new List<int>();
        private Queue<int> _moves = new Queue<int>();

        public EnemyStats stats;
        private EnemyAnimationController _animationController;

        public int StickNum { get; set; }
        private int _turnOrder;
        public int NextMove { get; set; }
        private int LastMove { get; set; }

        public List<Action> PreMovingEffects = new List<Action>();

        public int Direction
        {
            get
            {
                // -1 if moving backwards, 0 if not moving, 1 if moving forwards
                if (LastMove == 0) return 0;
                return LastMove / Math.Abs(LastMove);
            }
        }

        public bool Moving { get; private set; } = false;

        private EnemyTurnOrderText _turnOrderText;
        private EnemyHealthText _healthText;
        private TextMeshProUGUI _movementText;
        public Image image;
        public Image poisonImage;
        public TextMeshProUGUI poisonText;


        private EnemySpeechBubble _speechBubble;

        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;
        private const float Smooth = 0.01f;
        private Vector2 _aimPosition = Vector2.zero;

        public bool IsDestroyed { get; private set; } = false;

        public Stick Stick => StickManager.current.GetStick(StickNum);

        protected virtual void Awake()
        {
            image = transform.GetChild(0).GetComponent<Image>();
            _tooltipTrigger = transform.GetChild(1).GetComponent<TooltipTrigger>();
            _turnOrderText = transform.GetChild(2).GetComponent<EnemyTurnOrderText>();
            _healthText = transform.GetChild(3).GetComponent<EnemyHealthText>();
            _movementText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            poisonImage = transform.GetChild(5).GetComponent<Image>();
            poisonText = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
            _speechBubble = transform.GetChild(7).GetComponent<EnemySpeechBubble>();

            stats = new EnemyStats(this);
            _animationController = GetComponent<EnemyAnimationController>();

            Init();
        }

        protected abstract void Init();

        protected virtual void Start()
        {
            ChangeHealth(MaxHealth.Value);
            
            foreach (var move in MoveSet)
            {
                _moves.Enqueue(move);
            }

            // Randomise first move
            while (NextMove == 0)
            {
                var randomise = Random.Range(0, MoveSet.Count);

                for (var i = 0; i < randomise; i++)
                {
                    var move = _moves.Dequeue();
                    _moves.Enqueue(move);
                }

                SetNextMoveSequence();
            }

            BattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;

            _tooltipTrigger.header = Name;
        }

        private void LateUpdate()
        {
            if (TutorialManager.current.HighlightedEnemy) return;
        
            var localPosition = transform.localPosition;
            var newPositionX = Mathf.SmoothDamp(
                localPosition.x, _aimPosition.x, ref _moveVelocityX, Smooth);
            var newPositionY = Mathf.SmoothDamp(
                localPosition.y, _aimPosition.y, ref _moveVelocityY, Smooth);
            transform.localPosition = new Vector3(newPositionX, newPositionY, 0);
        }

        protected void SetInitialHealth(int health)
        {
            // TODO: APPLY MODIFIERS
            MaxHealth = new Stat(health);
        }

        public void AddMovementModifier(int amount)
        {
            for (var i = 0; i < MoveSet.Count; i++)
            {
                var move = MoveSet[i];
                if (move < 0)
                {
                    MoveSet[i] -= amount;
                }
                else if (move > 0)
                {
                    MoveSet[i] += amount;
                }
            }
        }

        private void OnEndEnemyTurn()
        {
            SetNextMoveSequence();
        }

        private void SetNextMoveSequence()
        {
            while (true)
            {
                var newMove = _moves.Dequeue();
                _moves.Enqueue(newMove);
                
                // Always move off starting stick
                if (newMove < 1 && StickNum == 0)
                {
                    continue;
                }
                
                NextMove = newMove;
                UpdateMovementText();

                break;
            }
        }

        public IEnumerator ExecuteMoveStep()
        {
            Moving = true;

            LastMove = NextMove;

            if (NextMove != 0 || !IsDestroyed)
            {

                // Change my stick
                StickNum += Direction;
                NextMove -= Direction;

                //HACK - Timing of sound clip
                // Wait to move
            
                InGameSfxManager.current.EnemyMoved();


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
                UpdateMovementText();
            }
            else
            {
                UpdateMovementText();
            }

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

            if (TestForPreMovingAbility())
            {
                PreMovingEffects.Add(PreMovingAbility);
            }
        
            _turnOrderText.MyTurn();

            // Move
        }

        public void EndMyTurn()
        {
            PreMovingEffects.Clear();
            _turnOrderText.EndMyTurn();
        }

        private void Poison()
        {
            DamageHandler.DamageEnemy(stats.Poison, this, DamageSource.Poison);
            InGameSfxManager.current.Poisoned();
        }

        protected virtual void PreMovingAbility()
        {
        }

        protected virtual bool TestForPreMovingAbility()
        {
            return false;
        }

        public void Block(int blockAmount)
        {
            if (NextMove > 0)
            {
                NextMove -= blockAmount;
                if (NextMove < 0) NextMove = 0;
            }
            else
            {
                NextMove += blockAmount;
                if (NextMove > 0) NextMove = 0;
            }

            UpdateMovementText();
        }

        public void MoveSprite(Vector2 newPosition)
        {
            _aimPosition = newPosition;
        }

        public void TakeDamage(int damage)
        {
            ChangeHealth(-damage);
            _animationController.Damage();
        }
    
        protected void Heal(int amount)
        {
            ChangeHealth(amount);
            InGameSfxManager.current.Healed();
            _animationController.Heal();
            BattleEvents.Current.EnemyHealed(this);
        }

        protected void ChangeHealth(int amount)
        {
            Health += amount;
            _healthText.AlterHealth(Health, MaxHealth.Value);

            if (Health <= 0)
            {
                DestroySelf(true);
            }
        }

        public void SetTurnOrder(int turnOrder)
        {
            _turnOrder = turnOrder;
            _turnOrderText.SetTurnOrder(_turnOrder);
        }

        protected void Speak(string text)
        {
            _speechBubble.WriteText(text);
        }

        public abstract string GetDescription();

        public void UpdateMovementText()
        {
            _movementText.text = NextMove.ToString();
        }

        public virtual void DestroySelf(bool killedByPlayer)
        {
            InGameSfxManager.current.Death();
            if (killedByPlayer) InventoryManager.current.AlterGold(Gold, "Enemy");
            IsDestroyed = true;
            Moving = false;
            Log.current.AddEvent("E" + _turnOrder + " has been killed");
            BattleEvents.Current.OnEndEnemyTurn -= OnEndEnemyTurn;
            ActiveEnemiesManager.Current.DestroyEnemy(this);
        }
    }
}
