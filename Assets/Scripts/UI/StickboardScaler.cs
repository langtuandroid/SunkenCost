using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickboardScaler : MonoBehaviour
{
    private Vector2 _resolution;

    private RectTransform _rectTransform;
 
    private void Awake()
    {
        _resolution = new Vector2(Screen.width, Screen.height);
        _rectTransform = GetComponent<RectTransform>();
        SetScale();
    }
 
    private void Update ()
    {
        if (Math.Abs(_resolution.x - Screen.width) > 1 || Math.Abs(_resolution.y - Screen.height) > 1)
        {
            // do your stuff
            SetScale();
            _resolution.x = Screen.width;
            _resolution.y = Screen.height;
        }
    }

    private void SetScale()
    {
        var scale = (16f / 11f) / (_resolution.x / _resolution.y);
        if (scale > 0.9f) scale = 0.9f;
        _rectTransform.localScale = new Vector3(scale, scale, 1);
    }
}
