using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleBoard;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ZoomManager : MonoBehaviour
{
    [SerializeField] private Transform _boardContentTransform;
    
    public static ZoomManager current;

    public float stickScale = 1f;
    private const float MinScale = 0.54f;
    private const float MaxScale = 1f;
    private const float ZoomSpeed = 3f;
    
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
        _gameCamera = GetComponent<Camera>();
        SetStickScale();
    }

    public void SetStickScale()
    {
        var stickCount = Board.Current.PlankCount;
        stickScale = stickCount <= 5 ? 1 : (1f - 0.13f - ((stickCount - 6) * 0.1f));
        _boardContentTransform.localScale =  new Vector3(stickScale, stickScale, 1);
    }
}
