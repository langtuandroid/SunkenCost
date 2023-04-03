using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Bson;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public enum MovementType
    {
        Walk,
        Skip,
        Wait
    }

    public struct EnemyMove
    {
        public MovementType movementType;
        public int magnitude;

        public void IncreaseMagnitude(int amountToIncrease)
        {
            if (magnitude == 0) return;

            if (magnitude > 0) magnitude += amountToIncrease;
            else magnitude -= amountToIncrease;
        }
    }
    
    public class EnemyMover : MonoBehaviour
    {
        public const int EnemyOffset = 100;
        private const float Smooth = 0.01f;

        [SerializeField] private EnemyUI.EnemyUI _enemyUI;
        
        private int _amountOfMovesLeftThisTurn;
        private int _lastMove;
        private int _skips;

        private List<EnemyMove> MoveSet = new List<EnemyMove>();
        private int _moveIndex;

        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;
        
        private Vector2 _aimPosition = Vector2.zero;

        public int StickNum { get; private set; }

        public bool FinishedMoving => _amountOfMovesLeftThisTurn == 0;

        private EnemyMove CurrentMove => MoveSet[_moveIndex];

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
                if (_amountOfMovesLeftThisTurn == 0) return 0;
                return _amountOfMovesLeftThisTurn / Math.Abs(_amountOfMovesLeftThisTurn);
            }
        }
        
        private void Start()
        {
            // Randomise first move
            _moveIndex = Random.Range(0, MoveSet.Count);
            SetNextMoveSequence();
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
            _lastMove = _amountOfMovesLeftThisTurn;
            StickNum += LastDirection + (LastDirection * _skips);
            _amountOfMovesLeftThisTurn -= LastDirection + (LastDirection * _skips);
            _skips = 0;
        }

        public void AddMove(MovementType moveType, int magnitude)
        {
            var move = new EnemyMove {movementType = moveType, magnitude = magnitude};
            MoveSet.Add(move);
        }

        public void AddMove(int magnitude)
        {
            AddMove(MovementType.Walk, magnitude);
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
            if (_amountOfMovesLeftThisTurn > 0)
            {
                _amountOfMovesLeftThisTurn -= amount;
                if (_amountOfMovesLeftThisTurn < 0) _amountOfMovesLeftThisTurn = 0;
            }
            else
            {
                _amountOfMovesLeftThisTurn += amount;
                if (_amountOfMovesLeftThisTurn > 0) _amountOfMovesLeftThisTurn = 0;
            }

            UpdateMovementText();
        }

        public void Reverse()
        {
            _amountOfMovesLeftThisTurn *= -1;
        }

        public void AddMovement(int amount)
        {
            _amountOfMovesLeftThisTurn += amount * NextDirection;
            
            if (_amountOfMovesLeftThisTurn <= -StickNum) _amountOfMovesLeftThisTurn = -StickNum;
            
            UpdateMovementText();
            
        }

        public void AddMovementModifier(int amount)
        {
            for (var i = 0; i < MoveSet.Count; i++)
            {
                var move = MoveSet.ElementAt(i);
                if (move.movementType == MovementType.Wait) continue;
                move.IncreaseMagnitude(amount);
            }
        }

        public void UpdateMovementText()
        {
            _enemyUI.MovementText.UpdateMovementText(CurrentMove.movementType, _amountOfMovesLeftThisTurn);
        }
        
        private void OnEndEnemyTurn() 
        {
            SetNextMoveSequence();
        }

        private void SetNextMoveSequence()
        {
            for (var i = 0; i < 100; i++)
            {
                _moveIndex++;
                if (_moveIndex >= MoveSet.Count) _moveIndex = 0;

                // Always move off starting stick
                if (MoveSet[_moveIndex].magnitude <= 0 && StickNum == 0)
                {
                    continue;
                }

                _amountOfMovesLeftThisTurn = CurrentMove.magnitude;
                _skips = CurrentMove.movementType == MovementType.Skip ? CurrentMove.magnitude - 1 : 0;
                UpdateMovementText();
                break;
            }
        }
    }
}