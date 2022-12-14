using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    private Image _image;
    private int _transitioning = 0;
    private const float FadeInSpeed = 0.005f;
    private const float FadeOutSpeed = 0.001f;
    private const float FadeInDelay = 0.7f;
    private const float Alpha = 0.75f;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
    }

    private void Start()
    {
        BattleEvents.Current.OnSticksUpdated += SticksUpdated;
    }

    public void SetColor(Color color)
    {
        /*
        Debug.Log("ENABLING");
        _transitioning = 1;
        _image.enabled = true;
        StartCoroutine(FadeIn(color));
        */
        
    }

    public void Hide()
    {
        /*
        _transitioning = -1;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
        */
    }
    
    private void SticksUpdated()
    {
        /*
        _transitioning = -1;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
        */
    }
    
    private IEnumerator FadeIn(Color color)
    {
        yield return new WaitForSeconds(FadeInDelay);
        
        for (var f = 0f; f < Alpha; f += 0.01f)
        {
            if (_transitioning != 1) yield break;
            _image.color = new Color(color.r, color.g, color.b, f);
            yield return new WaitForSeconds(FadeInSpeed);
        }

        // Successful finish
        if (_transitioning == 1) _transitioning = 0;
    }
    
    private IEnumerator FadeOut()
    {
        for (var f = _image.color.a; f > 0; f -= 0.01f)
        {
            if (f < 0f) f = 0f;
            if (_transitioning != -1) break;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, f);
            yield return new WaitForSeconds(FadeOutSpeed);
        }

        // Successful finish
        if (_transitioning == -1)
        {
            _image.enabled = false;
            _transitioning = 0;
        }
    }
    
    private void OnDestroy()
    {
        BattleEvents.Current.OnSticksUpdated -= SticksUpdated;
    }
}
