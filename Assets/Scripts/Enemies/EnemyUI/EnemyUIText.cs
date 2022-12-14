using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class EnemyUIText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    protected Color defaultColor;

    private const float ColorChangeTime = 0.5f;
    private bool _changingColor = false;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        defaultColor = _text.color;
    }

    protected void SetText(string text)
    {
        _text.text = text;
    }

    protected IEnumerator ChangeColorTemporarily(Color color)
    {
        // Let it finish any running coroutines first
        while (_changingColor) yield return 0;
        
        _text.color = color;
        _changingColor = true;
        yield return new WaitForSeconds(ColorChangeTime);
        _text.color = defaultColor;
        _changingColor = false;
    }

    protected void ChangeColor(Color color)
    {
        _text.color = color;
    }
}