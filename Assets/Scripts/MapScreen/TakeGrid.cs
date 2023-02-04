using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeGrid : MonoBehaviour
{
    [SerializeField] private Transform _boatTransform;

    public void OnTransformChildrenChanged()
    {
        var childCount = transform.childCount;
        
        if (childCount < 5)
        {
            _boatTransform.localPosition = new Vector3(230 * childCount - 550, _boatTransform.localPosition.y, 0);
        }
        else
        {
            _boatTransform.localPosition = new Vector3(550, _boatTransform.localPosition.y, 0);
        }
    }
}
