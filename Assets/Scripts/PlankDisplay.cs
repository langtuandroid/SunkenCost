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
        _plankImage.color = color;
        yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed);
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
        switch (battleEvent.type)
        {
            case BattleEventType.EtchingStunned:
            {
                var etching = BattleEventsManager.Current.GetEtchingByResponderID(battleEvent.affectedResponderID);
                if (etching.PlankNum == PlankNum)
                    SetAsStunned();
                break;
            }
            case BattleEventType.EtchingUnStunned:
            {
                var etching = BattleEventsManager.Current.GetEtchingByResponderID(battleEvent.affectedResponderID);
                if (etching.PlankNum == PlankNum)
                    SetAsUnStunned();
                break;
            }
            case BattleEventType.EtchingActivated when battleEvent.planksToColor.Contains(PlankNum):
            {
                var etching = BattleEventsManager.Current.GetEtchingByResponderID(battleEvent.affectedResponderID);
                StopAllCoroutines();
                StartCoroutine(ColorForAttackOnThisPlank(etching.design.Color));
                break;
            }
        }
    }
}