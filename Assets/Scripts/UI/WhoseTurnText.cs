using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhoseTurnText : MonoBehaviour
{
    public static WhoseTurnText current;

    [SerializeField] private TMP_ColorGradient yourColor;
    [SerializeField] private TMP_ColorGradient enemyColor;

    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void PlayersTurn()
    {
        //_textMeshProUGUI.text = "YOUR TURN";
        _textMeshProUGUI.colorGradientPreset = yourColor;
    }

    public void EnemiesTurn()
    {
        //_textMeshProUGUI.text = "ENEMY TURN";
        _textMeshProUGUI.colorGradientPreset = enemyColor;
    }

    public void Paused()
    {
        _textMeshProUGUI.text = "PAUSED";
        _textMeshProUGUI.colorGradientPreset = enemyColor;
    }
}
