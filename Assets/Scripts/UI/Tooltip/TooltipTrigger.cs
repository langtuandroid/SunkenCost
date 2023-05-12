using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;
    
    private void OnDestroy()
    {
        StopAllCoroutines();
        TooltipSystem.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ShowAfterDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TooltipSystem.Hide();
    }

    protected virtual string GetContent()
    {
        return content;
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        TooltipSystem.Show(GetContent(), header);
    }
}
