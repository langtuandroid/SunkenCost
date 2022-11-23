using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpeechBubble : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Image _image;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
    }

    public void WriteText(string text)
    {
        StartCoroutine(Activate(text));
    }

    private IEnumerator Activate(string text)
    {
        _text.text = text;
        _text.enabled = true;
        _image.enabled = true;
        
        yield return new WaitForSeconds(GameManager.AttackTime * 1.5f);

        _text.enabled = false;
        _image.enabled = false;
    }
}
