using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using BattleScreen.UI;
using Etchings;
using ReorderableContent;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(ReorderableElement))]
public class PlankDisplay : MonoBehaviour, IBattleEventUpdatedUI, IReorderableElementEventListener
{
    private static Color _stunnedColor = new Color(0.65f, 0.65f, 0.65f);

    [SerializeField] private Image _plankImage;
    [SerializeField] private Image _washImage;
    [SerializeField] private Image _indicatorImage;
    [SerializeField] private ReDrawButton _reDrawButton;

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
    
    public void RespondToBattleEvent(BattleEvent battleEvent)
    {
        switch (battleEvent.type)
        {
            case BattleEventType.EtchingStunned:
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.primaryResponderID);
                if (etching.PlankNum == PlankNum)
                    SetAsStunned();
                break;
            }
            case BattleEventType.EtchingUnStunned:
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.primaryResponderID);
                if (etching.PlankNum == PlankNum)
                    SetAsUnStunned();
                break;
            }
            case BattleEventType.EtchingActivated when battleEvent.planksToColor.Contains(PlankNum):
            {
                var etching = BattleEventResponseSequencer.Current.GetEtchingByResponderID(battleEvent.primaryResponderID);
                StopAllCoroutines();
                StartCoroutine(ColorForAttackOnThisPlank(etching.Design.Color));
                break;
            }
        }
    }

    public void Grabbed()
    {
        _washImage.enabled = false;
        _reDrawButton.Hide();
    }

    public void HoveringOverList(ReorderableGrid listHoveringOver)
    {
        return;
    }

    public void Released()
    {
        _washImage.enabled = true;
        _reDrawButton.Show();
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
}