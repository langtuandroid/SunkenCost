using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlankCount : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    [SerializeField] private TMP_ColorGradient _goodColor;
    [SerializeField] private TMP_ColorGradient _badColor;

    private void Awake()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        OfferScreenEvents.Current.OnGridsUpdated += UpdateText;
    }

    private void UpdateText()
    {
        var cardsOnTop = OfferManager.Current.CardsOnTopRow;
        var maxCards = GameProgress.MaxPlanks;

        _textMeshProUGUI.text = cardsOnTop + "/" + maxCards;

        _textMeshProUGUI.colorGradientPreset = cardsOnTop <= maxCards ? _goodColor : _badColor;
    }

    private void OnDestroy()
    {
        OfferScreenEvents.Current.OnGridsUpdated -= UpdateText;
    }
}
