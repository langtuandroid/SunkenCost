using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class OfferRow : MonoBehaviour
{
    public void ElementGrabbed(ReorderableList.ReorderableListEventStruct arg0)
    {
        OfferScreenEvents.Current.GridsUpdated();
    }

    public void ElementDropped(ReorderableList.ReorderableListEventStruct arg0)
    {
        OfferScreenEvents.Current.GridsUpdated();
    }
}
