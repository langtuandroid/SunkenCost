using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class OfferRow : MonoBehaviour
{
    private ReorderableList _reorderableList;

    public int grabbedCardIndex;
    private bool dragging;

    private void Awake()
    {
        _reorderableList = GetComponent<ReorderableList>();
        _reorderableList.IsDropable = false;
    }

    public void ElementGrabbed(ReorderableList.ReorderableListEventStruct arg0)
    {
        _reorderableList.IsDropable = true;
        dragging = true;
        grabbedCardIndex = arg0.FromIndex;
    }

    public void ElementDropped(ReorderableList.ReorderableListEventStruct arg0)
    {
        dragging = false;
        _reorderableList.IsDropable = false;
    }

    public void HoveredOver(int siblingIndex)
    {
        if (!dragging) return;
        transform.GetChild(0).GetChild(siblingIndex).SetParent(transform.parent.GetChild(transform.GetSiblingIndex()+1).GetChild(0).GetChild(0));
    }
}
