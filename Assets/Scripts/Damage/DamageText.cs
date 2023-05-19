using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Damage
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private float _tweenTime;
        [SerializeField] private float _zoomSize;
        [SerializeField] private float _yOffset;

        private Color _healColor = Color.green;
        private Color _damageColor = Color.red;
        
        private RectTransform _rectTransform;

        private Vector2 _defaultLocalPosition;
        private Vector2 _defaultLocalScale;
        private Vector2 _zoomScale;
        
        private void Awake()
        {
            _rectTransform = _textMeshProUGUI.GetComponent<RectTransform>();
            _defaultLocalScale = _rectTransform.localScale;
            _defaultLocalPosition = _rectTransform.localPosition;
            _zoomScale = new Vector2(_defaultLocalScale.x * _zoomSize, _defaultLocalScale.y * _zoomSize);

            _canvasGroup.alpha = 0f;
        }

        public void Heal(int amount)
        {
            Show(amount, _healColor, 1);
        }
        
        public void Damage(int initialDamage, DamageModificationPackage damageModificationPackage)
        {
            var multiTotal =
                damageModificationPackage.multiModifications.Aggregate
                    (1, (current, mod) => mod.modificationAmount * current);
            
            
            var sum = initialDamage + damageModificationPackage.flatModifications.Sum(i => i.modificationAmount) * multiTotal;
            Show(sum, _damageColor, -1);
        }

        private void Show(int amount, Color color, int sign)
        {
            _canvasGroup.alpha = 1f;
            _textMeshProUGUI.text = amount.ToString();
            _textMeshProUGUI.color = color;
            LeanTween.cancel(_rectTransform);
            _rectTransform.localScale = _defaultLocalScale;
            _rectTransform.localPosition = _defaultLocalPosition;
            LeanTween.scale(_rectTransform, _zoomScale * (0.7f + (amount / 20f)), _tweenTime  * Battle.ActionExecutionSpeed);
            LeanTween.moveLocalY(_rectTransform.gameObject, _defaultLocalPosition.y + (_yOffset * sign),
                _tweenTime * 2 * Battle.ActionExecutionSpeed);
        }

        public void Hide()
        {
            LeanTween.alphaCanvas(_canvasGroup, 0f, _tweenTime);
        }
    }
}