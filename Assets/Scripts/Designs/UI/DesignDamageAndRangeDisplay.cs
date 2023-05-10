using System;
using TMPro;
using UnityEngine;

namespace Designs.UI
{
    public class DesignDamageAndRangeDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform _damageRectTransform;
        [SerializeField] private GameObject _rangeGameObject;

        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _rangeText;

        private Design _design;

        public void Init(Design design)
        {
            _design = design;
            
            if (design.Category == DesignCategory.Effect) 
                throw new Exception("This gameObject should be deactivated!");

            if (design.Category == DesignCategory.MeleeAttack)
            {
                // Disable the ranged section and move the damage section to the center
                _rangeGameObject.SetActive(false);
                _damageRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _damageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _damageRectTransform.pivot = new Vector2(0.5f, 0.5f);
                _damageRectTransform.anchoredPosition = new Vector3(4, 0, 1);
            }
        }

        public void RefreshInfo()
        {
            Debug.Log(_design.Title);
            _damageText.text = _design.GetStat(StatType.Damage).ToString();
            
            if (_design.Category == DesignCategory.MeleeAttack) return;
            
            var minRange = _design.HasStat(StatType.MinRange) ? _design.GetStat(StatType.MinRange).ToString() : "0";
            var maxRange = _design.HasStat(StatType.MaxRange) ? _design.GetStat(StatType.MaxRange).ToString() : "\u221E";

            _rangeText.text = minRange + "-" + maxRange;
        }
    }
}