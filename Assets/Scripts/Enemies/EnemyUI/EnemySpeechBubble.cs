using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpeechBubble : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    private Image _image;

    private void Awake()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
    }

    public void DisplayText(string text)
    {
        StartCoroutine(Activate(text));
    }

    private IEnumerator Activate(string text)
    {
        _textMeshProUGUI.text = text;
        _textMeshProUGUI.enabled = true;
        _image.enabled = true;
        
        yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed * 1.5f);

        _textMeshProUGUI.enabled = false;
        _image.enabled = false;
    }
}
