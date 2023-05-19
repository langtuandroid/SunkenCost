using System;
using UnityEngine;

namespace UI
{
    public class BoatShaker : MonoBehaviour
    {
        [SerializeField] private float _tweenTime = 0.5f;
        [SerializeField] private float _xOffset = 20;
        
        private RectTransform _boatRectTransform;

        private Vector3 _defaultLocalPosition;
        private Vector3 _shakePosition;

        private void Awake()
        {
            _boatRectTransform = GetComponent<RectTransform>();
            _defaultLocalPosition = _boatRectTransform.localPosition;
            _shakePosition = new Vector3(_defaultLocalPosition.x + _xOffset, _defaultLocalPosition.y, 1);
        }

        public void Shake()
        {
            LeanTween.cancel(_boatRectTransform);
            _boatRectTransform.localPosition = _defaultLocalPosition;

            LeanTween.moveLocal(gameObject, _shakePosition, _tweenTime).setEasePunch();
        }
    }
}