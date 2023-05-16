using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BattleScreen.BattleBoard;
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

        private List<EnemyMove> _moveSet = new List<EnemyMove>();
        private int _moveIndex;

        public int PlankNum { get; private set; }

        public bool FinishedMoving => AmountOfMovesLeftThisTurn == 0;

        public EnemyMove CurrentMove { get; private set; }

        public int NextDirection
        {
            get
            {
                // -1 if moving backwards, 0 if not moving, 1 if moved forwards
                if (AmountOfMovesLeftThisTurn == 0) return 0;
                return AmountOfMovesLeftThisTurn / Math.Abs(AmountOfMovesLeftThisTurn);
            }
        }

        public bool WouldMoveOntoBoat => NextDirection + PlankNum >= Board.Current.PlankCount;

        public void Init(IEnumerable<EnemyMove> moveSet)
        {
            _moveSet = moveSet.ToList();
            
            // Randomise first move
            _moveIndex = Random.Range(0, _moveSet.Count);
            SetNextMove();
        }

        public void MoveToNextPlank()
        {
            int goalPlank;
            
            if (CurrentMove.MovementType == MovementType.Skip)
            {
                while (AmountOfMovesLeftThisTurn + PlankNum >= Board.Current.PlankCount) AmountOfMovesLeftThisTurn--;
                goalPlank = PlankNum + AmountOfMovesLeftThisTurn;
            }
            else
            {
                goalPlank = PlankNum + NextDirection;
            }
            
            if (EnemySequencer.Current.AllEnemies.Count(e => e.PlankNum == goalPlank) < 3)
            {
                PlankNum = goalPlank;
            }

            UseStep();
        }

        public void AttackBoat()
        {
            UseStep();
        }

        public void AddMove(MovementType movementType, int magnitude)
        {
            var move = new EnemyMove(movementType, magnitude);
            _moveSet.Add(move);
        }

        public void SetPlankNum(int plankNum)
        {
            PlankNum = plankNum;
        }
        
        public void Block(int amount)
        {
            AmountOfMovesLeftThisTurn -= amount * NextDirection;
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
                move.AlterMagnitude(amount);
            }
        }

        public void EndTurn() 
        {
            SetNextMove();
        }

        private void UseStep()
        {
            if (CurrentMove.MovementType == MovementType.Skip) AmountOfMovesLeftThisTurn = 0;
            else AmountOfMovesLeftThisTurn -= NextDirection;
        }

        private void SetNextMove()
        {
            for (var i = 0; i < 100; i++)
            {
                _moveIndex++;
                if (_moveIndex >= _moveSet.Count) _moveIndex = 0;

                CurrentMove = _moveSet[_moveIndex];

                // Always move off starting stick
                if (CurrentMove.MovementType != MovementType.Wait && CurrentMove.Magnitude <= 0 && PlankNum == -1)
                {
                    continue;
                }
                
                AmountOfMovesLeftThisTurn = CurrentMove.Magnitude;
                return;
            }
            
            throw new Exception("Couldn't find a move that moves off the island!");
        }
    }
}