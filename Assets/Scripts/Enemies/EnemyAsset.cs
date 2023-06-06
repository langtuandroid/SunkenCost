using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stats;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Enemy")]
    public class EnemyAsset : ScriptableObject
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private string _name;
        [SerializeField] private int _maxHealth;
        
        [Space(5)]
        [SerializeField] private List<EnemyMove> _moves = new List<EnemyMove>();

        [Space(5)] 
        [SerializeField] private List<EnemyModifierSetter> _modifierSetters = new List<EnemyModifierSetter>();
        public EnemyType EnemyType => _enemyType;
        public string Name => _name;
        public int MaxHealth => _maxHealth;
        public ReadOnlyCollection<EnemyMove> Moves => new ReadOnlyCollection<EnemyMove>(_moves.Select(m => m.GetMove).ToList());
        
        public Type Class { get; set; }
        public EnemySpritePack SpritePack { get; set; }

        public Dictionary<EnemyModifierType, int> Modifiers =>
            _modifierSetters.ToDictionary(modifierSetter => modifierSetter.ModifierType, 
                modifierSetter => modifierSetter.Magnitude);
    }
}