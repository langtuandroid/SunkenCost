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
        public int MovementRemainingThisTurn { get; private set; }

        private List<EnemyMove> _moveSet = new List<EnemyMove>();
        private int _moveIndex;

        public int PlankNum { get; private set; }

        public bool FinishedMoving => MovementRemainingThisTurn == 0;

        public EnemyMove CurrentMove { get; private set; }

        private int NextDirection
        {
            get
            {
                if (MovementRemainingThisTurn > 0) return 1;
                if (MovementRemainingThisTurn < 0) return -1;
                return 0;
            }
        }

        public bool WouldMoveOntoBoat => NextDirection + PlankNum >= Board.Current.PlankCount;

        public void Init(IEnumerable<EnemyMove> moveSet, int initialPlankNum)
        {
            SetPlankNum(initialPlankNum);
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
                while (MovementRemainingThisTurn + PlankNum >= Board.Current.PlankCount) MovementRemainingThisTurn--;
                goalPlank = PlankNum + MovementRemainingThisTurn;
            }
            else
            {
                goalPlank = PlankNum + NextDirection;
            }
            
            if (EnemySequencer.Current.GetIfPlankCanBeLandedOn(goalPlank))
            {
                SetPlankNum(goalPlank);
            }

            UseStep();
        }

        public void AttackBoat()
        {
            MovementRemainingThisTurn = 0;
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
            if (NextDirection == 1)
            {
                MovementRemainingThisTurn -= amount;
                if (MovementRemainingThisTurn < 0) MovementRemainingThisTurn = 0;
            }
            else
            {
                MovementRemainingThisTurn += amount;
                if (MovementRemainingThisTurn > 0) MovementRemainingThisTurn = 0;
            }
        }

        public void Reverse()
        {
            MovementRemainingThisTurn *= -1;
        }

        public void AddMovement(int amount)
        {
            MovementRemainingThisTurn += amount * NextDirection;
            
            if (MovementRemainingThisTurn < -PlankNum - 1) MovementRemainingThisTurn = -PlankNum - 1;
        }

        public void EndTurn() 
        {
            SetNextMove();
        }

        private void UseStep()
        {
            if (CurrentMove.MovementType == MovementType.Skip) MovementRemainingThisTurn = 0;
            else MovementRemainingThisTurn -= NextDirection;
        }

        private void SetNextMove()
        {
            for (var i = 0; i < 10000; i++)
            {
                _moveIndex++;
                if (_moveIndex >= _moveSet.Count) _moveIndex = 0;

                CurrentMove = _moveSet[_moveIndex];

                // Always move off starting stick
                if (CurrentMove.MovementType != MovementType.Wait && CurrentMove.Magnitude <= 0 && PlankNum == -1)
                {
                    continue;
                }
                
                MovementRemainingThisTurn = CurrentMove.Magnitude;
                return;
            }
            
            throw new Exception("Couldn't find a move that moves off the island!");
        }
    }
}