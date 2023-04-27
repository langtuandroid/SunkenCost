using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using Etchings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlankDisplay : MonoBehaviour, IBattleEventUpdatedUI
{
    private static Color _stunnedColor = new Color(0.65f, 0.65f, 0.65f);
    private Color _originalColor;
    
    [SerializeField] private Image _plankImage;
    [SerializeField] private Image _washImage;

    private int PlankNum => transform.GetSiblingIndex();

    private void Awake()
    {
        BattleRenderer.Current.RegisterUIUpdater(this);
        _originalColor = _plankImage.color;
        
        Debug.Log(PlankNum);
    }

    public void BeginDrag()
    {
        _washImage.enabled = false;
    }

    public void EndDrag()
    {
        _washImage.enabled = true;
    }

    private IEnumerator ColorForAttackOnThisPlank(Color color)
    {
        _plankImage.color = color;
        yield return new WaitForSeconds(Battle.ActionExecutionSpeed);
        _plankImage.color = _originalColor;
    }

    private void SetAsStunned()
    {
        _plankImage.color = _stunnedColor;
    }

    private void SetAsUnStunned()
    {
        _plankImage.color = Color.white;
    }

    public void RespondToBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent.etching && battleEvent.etching.PlankNum == PlankNum)
        {
            if (battleEvent.type == BattleEventType.EtchingStunned)
                SetAsStunned();
            else if (battleEvent.type == BattleEventType.EtchingUnStunned)
                SetAsUnStunned();
        }
        
        if (battleEvent.type == BattleEventType.EtchingActivated && battleEvent.planksToColor.Contains(PlankNum))
        {
            StopAllCoroutines();
            StartCoroutine(ColorForAttackOnThisPlank(battleEvent.etching.design.Color));
        }
    }
}