using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
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

    private Color _nextColor;
    private Queue<PlankDisplayState> _displayStates = new Queue<PlankDisplayState>();
    

    private void Awake()
    {
        BattleEventsManager.Current.RegisterUIUpdater(this);
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
        var oldColor = _plankImage.color;
        _plankImage.color = color;
        yield return new WaitForSeconds(Battle.ActionExecutionSpeed);
        _plankImage.color = oldColor;
    }

    private void SetAsStunned()
    {
        _plankImage.color = _stunnedColor;
    }

    private void SetAsUnStunned()
    {
        _plankImage.color = Color.white;
    }

    public bool GetIfUpdating(BattleEvent battleEvent)
    {
        if (!battleEvent.plankDisplays.Contains(this)) return false;
        
        _nextColor = battleEvent.type == BattleEventType.EtchingActivated
            ? battleEvent.etching.design.Color
            : Color.white;

        return true;

    }

    public void SaveStateResponse(BattleEventType battleEventType)
    {
        _displayStates.Enqueue(new PlankDisplayState(battleEventType, _nextColor));
    }

    public void LoadNextState()
    {
        var state = _displayStates.Dequeue();

        switch (state.type)
        {
            case BattleEventType.EtchingStunned:
                SetAsStunned();
                break;
            case BattleEventType.EtchingUnStunned:
                SetAsUnStunned();
                break;
            case BattleEventType.EnemyDamaged:
                StartCoroutine(ColorForAttackOnThisPlank(state.attackColor));
                break;
        }
    }

    private readonly struct PlankDisplayState
    {
        public readonly BattleEventType type;
        public readonly Color attackColor;

        public PlankDisplayState(BattleEventType type, Color attackColor)
        {
            this.type = type;
            this.attackColor = attackColor;
        }
    }
}