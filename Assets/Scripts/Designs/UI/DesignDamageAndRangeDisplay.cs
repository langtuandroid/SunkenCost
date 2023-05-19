using System;
using TMPro;
using UnityEngine;

namespace Designs.UI
{
    public class DesignDamageAndRangeDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        
        public void RefreshInfo(Design design)
        {
            var text = "<sprite=0> " + design.GetStat(StatType.Damage);

            if (design.Category != DesignCategory.MeleeAttack)
            {
                var minRange = design.HasStat(StatType.MinRange) 
                    ? design.GetStat(StatType.MinRange).ToString() : "0";
                var maxRange = design.HasStat(StatType.MaxRange) 
                    ? design.GetStat(StatType.MaxRange).ToString() : "<sprite=3>";

                string fullRange;
                
                if ((minRange == "0" && maxRange == "<sprite=3>") || minRange == maxRange)
                {
                    fullRange = maxRange;
                }
                else
                {
                    fullRange = minRange + "-" + maxRange;
                }

                text += "    <sprite=1> " + fullRange;
            }

            _textMeshProUGUI.text = text;
        }
    }
}