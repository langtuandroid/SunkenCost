using System;
using System.Collections;
using System.Collections.Generic;
using Etchings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Stick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image plankImage;
    [SerializeField] private Image washImage;
    
    private UnityEngine.UI.Extensions.ReorderableListNoEdges _parentListController;
    
    public ActiveEtching etching = null;
    public IndicatorController IndicatorController { get; private set; }
    private bool _dragging = false;
    public bool mouseIsOver = false;


    private void Start()
    {
        IndicatorController = GetComponentInChildren<IndicatorController>();

        _parentListController =
            transform.parent.parent.GetComponent<UnityEngine.UI.Extensions.ReorderableListNoEdges>();
    }

    public void Update()
    {
        if (_dragging)
        {
            transform.localScale = new Vector3(ZoomManager.current.stickScale, ZoomManager.current.stickScale, 1);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        /*
        if (PlayerController.current.IsOutOfMoves)
        {
            MoveText.current.NotEnoughMoves();
            InGameSfxManager.current.BadClick();
            return;
        }
        */

        washImage.enabled = false;
        _dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_dragging) return;
        transform.localScale = new Vector3(1, 1, 1);
        _dragging = false;
        washImage.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
    }

    public int GetStickNumber()
    {
        return transform.GetSiblingIndex();
    }

    public void SetTempColour(Color color)
    {
        StartCoroutine(ColorForAttack(color));
    }

    private IEnumerator ColorForAttack(Color color)
    {
        var oldColor = plankImage.color;
        plankImage.color = color;
        yield return new WaitForSeconds(BattleManager.AttackTime);
        plankImage.color = oldColor;
    }

    public void SetActive(bool active)
    {
        plankImage.color = active ? Color.white : new Color(0.65f, 0.65f, 0.65f);
    }

    public void DestroyStick()
    {
        var enemies = ActiveEnemiesManager.Current.GetEnemiesOnStick(GetStickNumber());

        foreach (var enemy in enemies)
        {
            enemy.DestroySelf(false);
        }
        
        InGameSfxManager.current.DestroyedPlank();
        Destroy(gameObject);
    }
}