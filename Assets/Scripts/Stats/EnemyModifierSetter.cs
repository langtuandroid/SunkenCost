using System;
using Designs;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class EnemyModifierSetter
    {
        [SerializeField] private EnemyModifierType modifierType;
        [SerializeField] private int _magnitude;

        public EnemyModifierType ModifierType => modifierType;
        public int Magnitude => _magnitude;
    }

    public enum EnemyModifierType
    {
        Health,
        Movement,
        Cooldown,
    }
}