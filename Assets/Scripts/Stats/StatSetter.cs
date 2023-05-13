using System;
using Designs;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class StatSetter
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private int _magnitude;

        public StatType StatType => _statType;
        public int Magnitude => _magnitude;
    }
}