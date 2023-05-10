using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace Designs
{
    [Serializable]
    public class StatSetter
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private int _magnitude;

        public StatType StatType => _statType;
        public int Magnitude => _magnitude;
    }
    
    [CreateAssetMenu(menuName = "Design")]
    public class DesignAsset : ScriptableObject
    {
        [FormerlySerializedAs("_designType")] [SerializeField] public DesignType designType;
        [SerializeField] public DesignCategory designCategory;
        [FormerlySerializedAs("_title")] [SerializeField] public string title;
        [FormerlySerializedAs("_sprite")] [SerializeField] public Sprite sprite;
        [SerializeField] public Color _color;
        [FormerlySerializedAs("_rarity")] [SerializeField] public Rarity rarity;
        [FormerlySerializedAs("_initialStats")] [SerializeField] public List<StatSetter> initialStats = new List<StatSetter>();
        [FormerlySerializedAs("_firstLevelUpStatMods")] [SerializeField] public List<StatSetter> firstLevelUpStatMods = new List<StatSetter>();
        [FormerlySerializedAs("_secondLevelUpStatMods")] [SerializeField] public List<StatSetter> secondLevelUpStatMods = new List<StatSetter>();
        
    }

    public enum DesignCategory
    {
        MeleeAttack,
        RangedAttack,
        AreaAttack,
        SpecialAttack,
        Effect,
    }
}