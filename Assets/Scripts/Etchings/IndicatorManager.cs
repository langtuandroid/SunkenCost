using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    private void Start()
    {
        BattleEvents.Current.OnSticksUpdated += SticksUpdated;
    }

    private void SticksUpdated()
    {
        foreach (var etching in EtchingManager.current.etchingOrder)
        {
            etching.UpdateIndicators();
        }
    }
}
