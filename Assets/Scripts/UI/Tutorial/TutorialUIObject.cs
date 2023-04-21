using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialUIObject : MonoBehaviour
{
    /*
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    
    private RectTransform _anchorTransform;
    private bool _hasAnchorTransform;

    private RectTransform _rectTransform;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        SetActive(false);
    }

    public void Move(float x, float y, int direction = 1)
    {
        SetActive(true);
        _anchorTransform = TutorialManager.current.HighlightedItemRectTransform;
        _hasAnchorTransform = _anchorTransform != null;
        var localScale = transform.localScale;
        localScale = new Vector3(direction * Mathf.Abs(localScale.x), localScale.y, 1);
        transform.localScale = localScale;
        _xOffset = x;
        _yOffset = y;
        
    }

    private void LateUpdate()
    {
        var newPosition = Vector2.zero;

        if (_hasAnchorTransform)
        {
            if (!TutorialManager.current.HighlightedItemRectTransform)
            {
                _hasAnchorTransform = false;
            }
            else
            {
                var loPos = _anchorTransform.localPosition;
                var ce = _anchorTransform.rect.center;
                newPosition = new Vector2(loPos.x + ce.x, loPos.y + ce.y);
            }
        }

        transform.localPosition = new Vector3(newPosition.x + _xOffset, newPosition.y + _yOffset, 0);
    }

    public void SetActive(bool active)
    {
        _canvasGroup.alpha = active ? 1 : 0;
        _canvasGroup.interactable = active;
        _canvasGroup.blocksRaycasts = active;
    }
    */
}
