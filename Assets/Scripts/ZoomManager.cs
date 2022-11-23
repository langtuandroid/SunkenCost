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

    private ScrollRect _scrollRect;

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
        stickScale = 1;

        var grid = FindObjectOfType<ReorderableListNoEdges>();
        _stickGridTransform = grid.Content.transform;
        _stickGridScrollRect = grid.GetComponent<ScrollRect>();
        _gameCamera = GetComponent<Camera>();

        _scrollRect = FindObjectOfType<ScrollRect>();
    }

    private void Update()
    {
        /*
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _scrollRect.horizontal = false;
        }
        else
        {
            _scrollRect.horizontal = true;
        }
        */
        /*
        var minXPosition = (1 - stickScale) * (5f / 6f) + (150 * StickManager.Current.stickCount);
        var stickGridLocalPosition = _stickGridTransform.localPosition;
        Debug.Log(minXPosition + " " + stickGridLocalPosition.x);

        if (stickGridLocalPosition.x > minXPosition)
        {
            _stickGridScrollRect.movementType = ScrollRect.MovementType.Elastic;
            _stickGridScrollRect.horizontal = false;
        }
        else if (_stickGridScrollRect.movementType == ScrollRect.MovementType.Elastic)
        {
            _stickGridScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            _stickGridScrollRect.horizontal = true;
        }
        */
    }

    private void LateUpdate()
    {
        //Debug.Log(_gameCamera.ScreenToWorldPoint(Input.mousePosition));
        
        /*
        if (Input.GetMouseButton(1))
        {
            var mousePos = _gameCamera.ScreenToViewportPoint(Input.mousePosition);
            _aimPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else
        {
            _aimPosition = new Vector2(_stickGridScrollRect.horizontalNormalizedPosition, _stickGridScrollRect.verticalNormalizedPosition);
        }
        */

        if (TutorialManager.current.InLockedTutorial) return;

        // Zooming
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f && Input.GetKey(KeyCode.LeftShift))
        {
            stickScale += scroll * ZoomSpeed;
            stickScale = Mathf.Clamp(stickScale, MinScale, MaxScale);
        }

        var currentLocalScale = _stickGridTransform.localScale;
        _targetScale = new Vector3(stickScale, stickScale, 1);
        
        /*
        _stickGridScrollRect.horizontalNormalizedPosition = Mathf.SmoothDamp(
            _stickGridScrollRect.horizontalNormalizedPosition, _aimPosition.x, ref _scrollVelocityHorizontal, Smooth);
        
        _stickGridScrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(
            _stickGridScrollRect.verticalNormalizedPosition, _aimPosition.y, ref _scrollVelocityVertical, Smooth);
            */

        _stickGridTransform.localScale = Vector3.SmoothDamp(currentLocalScale,
            _targetScale, ref _zoomVelocity, Smooth);

    }
}
