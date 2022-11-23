using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesignCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Design design;
    private DesignInfo _designInfo;
    
    public bool hoveredOver;
    private Vector2 _normalPosition;
    private Quaternion _normalRotation;
    
    private float _moveVelocityY = 0f;
    private const float Smooth = 0.01f;
    private float _aimYPosition;
    private Quaternion _aimRotation;

    private bool _dragging;

    private Camera _camera;
    private Stick _stickHoveredOver;
    private bool _hoveringOverStick;
    private CanvasGroup _canvasGroup;

    private Stick _stickWithDummyEtching;

    private void Awake()
    {
        _camera = Camera.main;
        _canvasGroup = GetComponent<CanvasGroup>();
        _designInfo = transform.GetChild(1).GetComponent<DesignInfo>();
    }

    private void Start()
    {
        _designInfo.design = design;
    }

    private void Update()
    {
        if (_dragging) return;

        if (!hoveredOver)
        {
            _aimYPosition = _normalPosition.y;
            _aimRotation = _normalRotation;
        }

        var t = transform;
        var localPosition = t.localPosition;


        if (Mathf.Abs(_aimYPosition - localPosition.y) > 0.01f)
        {
            var newPositionY = Mathf.SmoothDamp(
                localPosition.y, _aimYPosition, ref _moveVelocityY, Smooth);
            t.localPosition = new Vector3(_normalPosition.x, newPositionY, 0);
        }
        
        
        /*
        var localRotation = t.localEulerAngles.z;
        if (Mathf.Abs(_aimRotation - localRotation) < 0.01f) return;
            
        var newRotationZ = Mathf.SmoothDamp(
            localRotation, _aimRotation, ref _moveRotation, Smooth);
        t.localEulerAngles = new Vector3(0, 0, newRotationZ);*/

        t.localRotation = Quaternion.Slerp(t.localRotation, _aimRotation, 20f * Time.deltaTime);

    }

    private void HoveredOver()
    {
        hoveredOver = true;
        _aimYPosition = 100;
        _aimRotation = Quaternion.identity;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoveredOver();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!hoveredOver) HoveredOver();
        else hoveredOver = false;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PlayerController.current.MovesRemaining < design.GetStat(St.Cost)) return;
        
        Bench.current.DraggedCard(this);
        _dragging = true;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_dragging) return;
        
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        
        _stickHoveredOver = StickManager.current.MouseOver;
        
        if (_stickHoveredOver)
        {
            if (_hoveringOverStick) return;

            _canvasGroup.alpha = 0;
            _hoveringOverStick = true;
            EtchingManager.current.CreateDummyEtching(_stickHoveredOver, design);
            _stickWithDummyEtching = _stickHoveredOver;
        }
        else if (_hoveringOverStick)
        {
            _hoveringOverStick = false;
            _canvasGroup.alpha = 1;
            if (_stickWithDummyEtching)
            {
                EtchingManager.current.DestroyDummyEtching(_stickWithDummyEtching);
                _stickWithDummyEtching = null;
            }
        } 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;
        
        if (!_hoveringOverStick)
        {
            Bench.current.ReAddCard(this);
            _canvasGroup.blocksRaycasts = true;
            return;
        }
        
        EtchingManager.current.CreateEtching(_stickHoveredOver, design);
        PlayerController.current.PlayCard(design.GetStat(St.Cost));
        Destroy(gameObject);
    }

    public void BenchUpdated()
    {
        hoveredOver = false;
        var t = transform;
        _normalPosition = t.localPosition;
        _normalRotation = t.localRotation;
    }
}
