using System;
using System.Collections;
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
        [SerializeField] private EnemyUI.EnemyUI _enemyUI;
        
        public int AmountOfMovesLeftThisTurn { get; private set; }
        private int _lastMove;
        private int _skips;

        private List<EnemyMove> MoveSet = new List<EnemyMove>();
        private int _moveIndex;

        
        public int PlankNum { get; private set; }

        public bool FinishedMoving => AmountOfMovesLeftThisTurn == 0;

        public EnemyMove CurrentMove => MoveSet[_moveIndex];

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
                if (AmountOfMovesLeftThisTurn == 0) return 0;
                return AmountOfMovesLeftThisTurn / Math.Abs(AmountOfMovesLeftThisTurn);
            }
        }
        
        private void Start()
        {
            // Randomise first move
            _moveIndex = Random.Range(0, MoveSet.Count);
            SetNextMoveSequence();
        }
        
        public void Move()
        {
            _lastMove = AmountOfMovesLeftThisTurn;
            PlankNum += LastDirection + (LastDirection * _skips);
            AmountOfMovesLeftThisTurn -= LastDirection + (LastDirection * _skips);
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

        public void SetPlankNum(int plankNum)
        {
            PlankNum = plankNum;
        }

        public void AddSkip(int amount)
        {
            _skips += amount;
        }

        public void Block(int amount)
        {
            if (AmountOfMovesLeftThisTurn > 0)
            {
                AmountOfMovesLeftThisTurn -= amount;
                if (AmountOfMovesLeftThisTurn < 0) AmountOfMovesLeftThisTurn = 0;
            }
            else
            {
                AmountOfMovesLeftThisTurn += amount;
                if (AmountOfMovesLeftThisTurn > 0) AmountOfMovesLeftThisTurn = 0;
            }

            UpdateMovementText();
        }

        public void Reverse()
        {
            AmountOfMovesLeftThisTurn *= -1;
        }

        public void AddMovement(int amount)
        {
            AmountOfMovesLeftThisTurn += amount * NextDirection;
            
            if (AmountOfMovesLeftThisTurn <= -PlankNum) AmountOfMovesLeftThisTurn = -PlankNum;
            
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
            _enemyUI.MovementText.UpdateMovementText(CurrentMove.movementType, AmountOfMovesLeftThisTurn);
        }
        
        public void EndTurn() 
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
                if (MoveSet[_moveIndex].magnitude <= 0 && PlankNum == 0)
                {
                    continue;
                }

                AmountOfMovesLeftThisTurn = CurrentMove.magnitude;
                _skips = CurrentMove.movementType == MovementType.Skip ? CurrentMove.magnitude - 1 : 0;
                UpdateMovementText();
                break;
            }
        }
    }
}