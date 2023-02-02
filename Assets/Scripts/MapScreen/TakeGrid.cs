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

        Debug.Log(childCount);
        
        if (childCount < 5)
        {
            _boatTransform.localPosition = new Vector3(250 * childCount, _boatTransform.localPosition.y, 0);
        }
        else
        {
            _boatTransform.localPosition = new Vector3(1100, _boatTransform.localPosition.y, 0);
        }
    }
}
