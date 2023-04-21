using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorController : MonoBehaviour
{
    private Image _image;

    [SerializeField] private Color _normalColour;
    [SerializeField] private Color _bossColor;

    private void Start()
    {
        _image = GetComponent<Image>();
        _normalColour = _image.color;
    }

    private void OnBossSpawned()
    {
        _image.color = _bossColor;
    }
    private void OnBossKilled()
    {
        _image.color = _normalColour;
    }
}
