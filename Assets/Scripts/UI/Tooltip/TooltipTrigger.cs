using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;

    private static LTDescr _delay;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.Show(GetContent(), header);
        });

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(_delay.uniqueId);
        TooltipSystem.Hide();
    }

    private void OnDestroy()
    {
        TooltipSystem.Hide();
    }

    protected virtual string GetContent()
    {
        return content;
    }
}
