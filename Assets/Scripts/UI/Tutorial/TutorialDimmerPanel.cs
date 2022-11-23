using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDimmerPanel : MonoBehaviour
{
    public static TutorialDimmerPanel current;

    private Image _image;

    private void Awake()
    {
        current = this;

        _image = GetComponent<Image>();
    }

    public void SetVisible(bool isVisible)
    {
        _image.enabled = isVisible;

        //StartCoroutine(FadeAlpha(isVisible));
    }

    private IEnumerator FadeAlpha(bool isVisible)
    {
        var targetAlpha = isVisible ? 1f : 0f;
        var startAlpha= _image.color.a;
        var t = 0f;

        while (_image.color.a - targetAlpha > 0.01f)
        {
            var currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            var color = _image.color;
            color = new Color(color.r, color.b, color.g, currentAlpha);
            _image.color = color;

            t += 0.01f;
            yield return 0;
        }
    }
}
