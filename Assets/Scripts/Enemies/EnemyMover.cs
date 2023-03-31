using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Bson;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyMover : MonoBehaviour
    {
        public const int EnemyOffset = 100;
        private const float Smooth = 0.01f;

        [SerializeField] private EnemyUI.EnemyUI _enemyUI;
        
        private int _nextMove;
        private int _lastMove;
        private int _skips;

        private List<int> MoveSet = new List<int>();
        private Queue<int> _moves = new Queue<int>();

        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;
        
        private Vector2 _aimPosition = Vector2.zero;

        public int StickNum { get; private set; } = 0;

        public bool FinishedMoving => _nextMove == 0;

        public int LastDirection
        {
            get
            {
                // -1 if moved backwards, 0 if didn't move, 1 if moved forwards
                if (_lastMove == 0) return 0;
                return _lastMove / Math.Abs(_lastMove);
            }
        }

        public int NextDirection
        {
            get
            {
                // -1 if moving backwards, 0 if not moving, 1 if moved forwards
                if (_nextMove == 0) return 0;
                return _nextMove / Math.Abs(_nextMove);
            }
        }
        
        private void Start()
        {
            _moves.Clear();
            foreach (var move in MoveSet)
            {
                _moves.Enqueue(move);
            }

            // Randomise first move
            foreach (var randomise in MoveSet.Select(i => Random.Range(0, MoveSet.Count)))
            {
                for (var ii = 0; ii < randomise; ii++)
                {
                    var move = _moves.Dequeue();
                    _moves.Enqueue(move);
                }

                SetNextMoveSequence();
            }
            
            BattleEvents.Current.OnEndEnemyTurn += OnEndEnemyTurn;
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

        private void OnDestroy()
        {
            BattleEvents.Current.OnEndEnemyTurn -= OnEndEnemyTurn;
        }

        public void Move()
        {
            _lastMove = _nextMove;
            StickNum += LastDirection + (LastDirection * _skips);
            _skips = 0;
            _nextMove -= LastDirection;
        }

        public void AddMove(int move)
        {
            MoveSet.Add(move);
        }

        public void SetStickNum(int stickNum)
        {
            StickNum = stickNum;
        }

        public void SetAimPosition(Vector2 newPosition)
        {
            _aimPosition = newPosition;
        }

        public void AddSkip(int amount)
        {
            _skips += amount;
        }

        public void Block(int amount)
        {
            if (_nextMove > 0)
            {
                _nextMove -= amount;
                if (_nextMove < 0) _nextMove = 0;
            }
            else
            {
                _nextMove += amount;
                if (_nextMove > 0) _nextMove = 0;
            }

            UpdateMovementText();
        }

        public void Reverse()
        {
            _nextMove *= -1;
        }

        public void AddMovement(int amount)
        {
            _nextMove += amount * NextDirection;
            
            if (_nextMove <= -StickNum) _nextMove = -StickNum;
            
            UpdateMovementText();
            
        }

        public void AddMovementModifier(int amount)
        {
            var moveSet = _moves.ToArray();
            
            for (var i = 0; i < moveSet.Length; i++)
            {
                var move = moveSet[i];
                if (move < 0)
                {
                    moveSet[i] -= amount;
                }
                else if (move > 0)
                {
                    moveSet[i] += amount;
                }
            }
            
            _moves.Clear();
            foreach (var move in moveSet)
            {
                _moves.Enqueue(move);
            }
        }

        public void UpdateMovementText()
        {
            _enemyUI.MovementText.UpdateMovementText(_nextMove);
        }
        
        private void OnEndEnemyTurn()
        {
            SetNextMoveSequence();
        }
        
        private void SetNextMoveSequence()
        {
            var newMove = _moves.Dequeue();
            _moves.Enqueue(newMove);
            
            // Always move off starting stick
            if (newMove < 1 && StickNum == 0)
            {
                newMove = _moves.Dequeue();
                _moves.Enqueue(newMove);
            }
                
            _nextMove = newMove;
            UpdateMovementText();
        }
    }
}