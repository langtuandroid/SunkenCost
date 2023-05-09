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

    public class EnemyMover
    {
        public int AmountOfMovesLeftThisTurn { get; private set; }
        private int _lastMove;
        private int _skips;

        private readonly List<EnemyMove> _moveSet = new List<EnemyMove>();
        private int _moveIndex;

        
        public int PlankNum { get; private set; }

        public bool FinishedMoving => AmountOfMovesLeftThisTurn == 0;

        public EnemyMove CurrentMove => _moveSet[_moveIndex];

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
        
        public void Init()
        {
            // Randomise first move
            _moveIndex = Random.Range(0, _moveSet.Count);
            SetNextMoveSequence();
        }
        
        public void Move()
        {
            _lastMove = AmountOfMovesLeftThisTurn;
            PlankNum += LastDirection + (LastDirection * _skips);
            AmountOfMovesLeftThisTurn -= LastDirection + (LastDirection * _skips);
            _skips = 0;
        }

        public void AddMove(MovementType movementType, int magnitude)
        {
            var move = new EnemyMove(movementType, magnitude);
            _moveSet.Add(move);
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
        }

        public void Reverse()
        {
            AmountOfMovesLeftThisTurn *= -1;
        }

        public void AddMovement(int amount)
        {
            AmountOfMovesLeftThisTurn += amount * NextDirection;
            
            if (AmountOfMovesLeftThisTurn < -PlankNum - 1) AmountOfMovesLeftThisTurn = -PlankNum - 1;
        }

        public void AddMovementModifier(int amount)
        {
            for (var i = 0; i < _moveSet.Count; i++)
            {
                var move = _moveSet.ElementAt(i);
                if (move.MovementType == MovementType.Wait) continue;
                move.IncreaseMagnitude(amount);
            }
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
                if (_moveIndex >= _moveSet.Count) _moveIndex = 0;

                // Always move off starting stick
                if (_moveSet[_moveIndex].Magnitude <= 0 && PlankNum == -1)
                {
                    continue;
                }
                
                Debug.Log(CurrentMove.Magnitude);
                AmountOfMovesLeftThisTurn = CurrentMove.Magnitude;
                _skips = CurrentMove.MovementType == MovementType.Skip ? CurrentMove.Magnitude - 1 : 0;
                break;
            }
        }
    }
}