using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesignCardInfo : DesignInfo
{
    [SerializeField] private TextMeshProUGUI _costText;

    [SerializeField] private Color _goodColor;
    [SerializeField] private Color _badColor;

    protected override void Start()
    {
        GameEvents.current.OnPlayerMovedStick += UpdateCostText;
        GameEvents.current.OnPlayerMovedStick += UpdateCostText;
        base.Start();
    }
    
    protected override void Init()
    {
        UpdateCostText();
        base.Init();
    }

    private void UpdateCostText()
    {
        _costText.color = GetColor();
        _costText.text = design.GetStat(St.Cost).ToString();
    }

    private Color GetColor()
    {
        return PlayerController.current.MovesRemaining >= design.GetStat(St.Cost) ? _goodColor : _badColor;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPlayerMovedStick -= UpdateCostText;
        GameEvents.current.OnPlayerMovedStick -= UpdateCostText;
    }
}
