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

    [SerializeField] private Image _plankImage;
    [SerializeField] private Image _washImage;
    [SerializeField] private Image _indicatorImage;

    private Color _originalPlankColor;
    private Color _originalIndicatorColor;
    private float _originalIndicatorAlpha;
    
    private bool _isStunned;

    private int PlankNum => transform.GetSiblingIndex();

    private void Awake()
    {
        BattleRenderer.Current.RegisterUIUpdater(this);
        _originalPlankColor = _plankImage.color;
        _originalIndicatorColor = _indicatorImage.color;
        _originalIndicatorAlpha = _originalIndicatorColor.a;
        _isStunned = false;
    }
    
    private void OnDestroy()
    {
        if (BattleRenderer.Current)
            BattleRenderer.Current.DeregisterUIUpdater(this);
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
        _indicatorImage.color = new Color(color.r, color.g, color.b, _originalIndicatorAlpha);
        yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed);
        _indicatorImage.color = _isStunned? new Color(0, 0, 0, 0) : _originalIndicatorColor;
        
    }

    private void SetAsStunned()
    {
        _isStunned = true;
        _plankImage.color = _stunnedColor;
        _indicatorImage.color = new Color(0, 0, 0, 0);
    }

    private void SetAsUnStunned()
    {
        _isStunned = false;
        _plankImage.color = _originalPlankColor;
        _indicatorImage.color = _originalIndicatorColor;
    }

    public void RespondToBattleEvent(BattleEvent battleEvent)
    {
        switch (battleEvent.type)
        {
            case BattleEventType.EtchingStunned:
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.creatorID);
                if (etching.PlankNum == PlankNum)
                    SetAsStunned();
                break;
            }
            case BattleEventType.EtchingUnStunned:
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.creatorID);
                if (etching.PlankNum == PlankNum)
                    SetAsUnStunned();
                break;
            }
            case BattleEventType.EtchingActivated when battleEvent.planksToColor.Contains(PlankNum):
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.creatorID);
                StopAllCoroutines();
                StartCoroutine(ColorForAttackOnThisPlank(etching.design.Color));
                break;
            }
        }
    }
}