using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
using Etchings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Plank : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _plankImage;
    [SerializeField] private Image _washImage;

    public Etching Etching { get; set; }

    private bool _dragging = false;
    public bool MouseIsOver { get; private set; }

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
        
        _washImage.enabled = false;
        _dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_dragging) return;
        transform.localScale = new Vector3(1, 1, 1);
        _dragging = false;
        _washImage.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseIsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseIsOver = false;
    }

    public int GetPlankNum()
    {
        return transform.GetSiblingIndex();
    }

    public void SetTempColour(Color color)
    {
        StartCoroutine(ColorForAttack(color));
    }

    private IEnumerator ColorForAttack(Color color)
    {
        var oldColor = _plankImage.color;
        _plankImage.color = color;
        yield return new WaitForSeconds(Battle.ActionExecutionSpeed);
        _plankImage.color = oldColor;
    }

    public void SetActiveColor(bool active)
    {
        _plankImage.color = active ? Color.white : new Color(0.65f, 0.65f, 0.65f);
    }

    public void DestroyPlank(DamageSource source)
    {
        var enemies = EnemyController.Current.GetEnemiesOnStick(GetPlankNum());

        foreach (var enemy in enemies)
        {
            enemy.DestroySelf(source);
        }
        Destroy(gameObject);
    }
}