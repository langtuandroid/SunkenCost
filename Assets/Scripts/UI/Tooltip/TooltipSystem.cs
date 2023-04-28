using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem _current;
    public Tooltip tooltip;

    public void Awake()
    {
        if (_current)
            Destroy(_current.gameObject);
        _current = this;
    }

    public static void Show(string content, string header = "")
    {
        _current.tooltip.SetText(content, header);
        _current.tooltip.SetActive(true);
    }

    public static void Hide()
    {
        if (_current.tooltip)
            _current.tooltip.SetActive(false);
    }
}
