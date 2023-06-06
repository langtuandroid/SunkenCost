using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class EnemyMove
    {
        [SerializeField] private MovementType _movementType;
        [SerializeField] private int _magnitude;

        public MovementType MovementType => _movementType;
        public int Magnitude => _magnitude;

        public EnemyMove(MovementType movementType, int magnitude)
        {
            _movementType = movementType;
            _magnitude = magnitude;
        }

        public void AlterMagnitude(int amountToIncrease)
        {
            if (_magnitude == 0) return;

            if (_magnitude > 0) _magnitude += amountToIncrease;
            else _magnitude -= amountToIncrease;
        }
        
        public EnemyMove GetMove => new EnemyMove(_movementType, _magnitude);
    }
}