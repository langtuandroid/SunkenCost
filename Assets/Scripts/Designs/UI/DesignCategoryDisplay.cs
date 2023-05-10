using TMPro;
using UnityEngine;

namespace Designs.UI
{
    public class DesignCategoryDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private TooltipTrigger _tooltipTrigger;

        private bool _isEffect;

        public void Init(DesignCategory designCategory)
        {
            var title = "";
            var description = "";
            var color = Color.black;
            
            switch (designCategory)
            {
                case DesignCategory.MeleeAttack:
                    title = "Melee Attack";
                    description = "Attacks an enemy when it lands on this plank";
                    color = new Color(0f, 0.5f, 0.5f);
                    break;
                case DesignCategory.RangedAttack:
                    title = "Ranged Attack";
                    description = "Attacks an enemy when it lands within range";
                    color = new Color(0.5f, 0f, 0f);
                    break;
                case DesignCategory.AreaAttack:
                    title = "Area Attack";
                    description = "Attacks all enemies in range when an enemy lands within range";
                    color = new Color(0.5f, 0.5f, 0f);
                    break;
                case DesignCategory.SpecialAttack:
                    title = "Special Attack";
                    color = new Color(0.1f, 0.5f, 0.1f);
                    _tooltipTrigger.enabled = false;
                    break;
                case DesignCategory.Effect:
                    title = "Effect";
                    color = new Color(0.5f, 0f, 0.5f);
                    _tooltipTrigger.enabled = false;
                    break;
            }

            _textMeshProUGUI.text = _tooltipTrigger.header = title;
            _textMeshProUGUI.color = color;
            _tooltipTrigger.content = description;
        }
    }
}