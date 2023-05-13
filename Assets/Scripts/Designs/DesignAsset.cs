using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace Designs
{
    [CreateAssetMenu(menuName = "Design")]
    public class DesignAsset : ScriptableObject
    {
        [FormerlySerializedAs("_designType")] [SerializeField] public DesignType designType;
        [SerializeField] public DesignCategory designCategory;
        [FormerlySerializedAs("_title")] [SerializeField] public string title;
        [FormerlySerializedAs("_sprite")] [SerializeField] public Sprite sprite;
        [SerializeField] public Color _color;
        [FormerlySerializedAs("_rarity")] [SerializeField] public Rarity rarity;

        [Space(10)] 
        [Header("Description")]
        [Space(5)]
        public string description = "replacementStats replace @, then #, then *";

        public string notUpgradedText = "";
        public string firstUpgradeText = "";
        public string secondUpgradeText = "";
        public ExtraTextSetting extraTextSetting;

        public List<StatType> replacementStats = new List<StatType>();
        
        
        [Space (10)]
        [Header("Stats")]
        [Space(5)]
        [FormerlySerializedAs("_initialStats")] [SerializeField] public List<StatSetter> initialStats = new List<StatSetter>();
        [FormerlySerializedAs("_firstLevelUpStatMods")] [SerializeField] public List<StatSetter> firstLevelUpStatMods = new List<StatSetter>();
        [FormerlySerializedAs("_secondLevelUpStatMods")] [SerializeField] public List<StatSetter> secondLevelUpStatMods = new List<StatSetter>();

        public string GetDescription(Design design)
        {
            var levelSpecificDesc = GetDescriptionByDesignLevel(design.Level);

            var statReplacedDesc = ReplaceSymbolsWithStats(design, levelSpecificDesc);
            
            statReplacedDesc = statReplacedDesc.Replace("1x ", "");
            statReplacedDesc = statReplacedDesc.Replace("2x", "double");
            statReplacedDesc = statReplacedDesc.Replace("3x", "triple");

            if (design.HasStat(StatType.UsesPerTurn))
            {
                var usesPerTurn = design.GetStat(StatType.UsesPerTurn);
                var usesText = usesPerTurn switch
                {
                    1 => "once",
                    2 => "twice",
                    _ => usesPerTurn + "x"
                };
                statReplacedDesc += " (" + usesText + " per turn)";
            }
        
            var descriptionWithSprites = statReplacedDesc
                .Replace("damage", "<sprite=0>")
                .Replace("movement", "<sprite=2>");
        
            return descriptionWithSprites;
        }

        private string GetDescriptionByDesignLevel(int level)
        {
            var upgradeDescriptions = new List<string>() {notUpgradedText, firstUpgradeText, secondUpgradeText};
            var descriptionsOfThisLevelAndBelow = 
                upgradeDescriptions.Where((desc, index) => index <= level);
            
            var lastApplicableDescription = descriptionsOfThisLevelAndBelow.LastOrDefault(desc => desc != "");
            
            if (lastApplicableDescription is null)
            {
                return description;
            }
            
            return extraTextSetting == ExtraTextSetting.Extend
                ? description + lastApplicableDescription
                : lastApplicableDescription;

        }

        private string ReplaceSymbolsWithStats(Design design, string desc)
        {
            if (replacementStats.Count == 0) return desc;

            desc = desc.Replace("@", Math.Abs(design.GetStat(replacementStats[0])).ToString());
            if (replacementStats.Count == 1) return desc;
            desc = desc.Replace("#", Math.Abs(design.GetStat(replacementStats[1])).ToString());
            if (replacementStats.Count == 2) return desc;
            desc = desc.Replace("*", Math.Abs(design.GetStat(replacementStats[2])).ToString());
            return desc;

        }
    }

    public enum DesignCategory
    {
        MeleeAttack,
        RangedAttack,
        AreaAttack,
        SpecialAttack,
        Effect,
    }

    public enum ExtraTextSetting
    {
        Extend,
        Replace
    }
}