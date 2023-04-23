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

public class PlankDisplay : MonoBehaviour
{
    [SerializeField] private Image _plankImage;
    [SerializeField] private Image _washImage;

    public Etching Etching { get; set; }

    public void BeginDrag()
    {
        _washImage.enabled = false;
    }

    public void EndDrag()
    {
        _washImage.enabled = true;
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
}