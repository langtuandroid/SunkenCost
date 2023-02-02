using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardGrid : MonoBehaviour
{
    private GridLayoutGroup _gridLayoutGroup;

    private ReorderableListContent _reorderableListContent;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        UpdateSpacing();
    }

    public void OnTransformChildrenChanged()
    {
        UpdateSpacing();
    }

    private void UpdateSpacing()
    {
        var childCount = transform.childCount;

        if (childCount > 4)
        {
            var totalOverflow = 1100 - (250 * childCount);
            var spacing = (float) totalOverflow / (childCount - 1);
            Debug.Log(spacing);
            _gridLayoutGroup.spacing = new Vector2(spacing, 0);
        }
        else
        {
            _gridLayoutGroup.spacing = new Vector2(0, 0);
        }
    }
}
