using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ZoomManager : MonoBehaviour
{
    public static ZoomManager current;
    
    
    public float stickScale = 1f;
    private const float MinScale = 0.54f;
    private const float MaxScale = 1f;
    private const float ZoomSpeed = 3f;

    private Transform _stickGridTransform;
    private ScrollRect _stickGridScrollRect;
    
    private Vector3 _targetScale;
    private Camera _gameCamera;
    private Vector3 _zoomVelocity = Vector3.zero;
    private const float Smooth = 0.01f;
    private Vector2 _aimPosition = Vector2.zero;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    void Start()
    {
        var grid = FindObjectOfType<ReorderableListNoEdges>();
        _stickGridTransform = grid.Content.transform;
        _gameCamera = GetComponent<Camera>();
        
        SetStickScale();
        
        OldBattleEvents.Current.OnStickAdded += SetStickScale;
    }

    private void SetStickScale()
    {
        var stickCount = StickManager.current.stickCount;
        stickScale = stickCount <= 5 ? 1 : (1f - 0.13f - ((stickCount - 6) * 0.1f));
        _stickGridTransform.localScale =  new Vector3(stickScale, stickScale, 1);
    }
}
