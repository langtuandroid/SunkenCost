using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Designs.UI;
using Etchings;
using UnityEngine;

namespace Designs
{
    public class EtchingDisplay : MonoBehaviour, IBattleEventUpdatedUI
    {
        private Color _normalColor;

        private Etching _etching;
        private DesignDisplay _designDisplay;

        private void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
            
            _etching = GetComponent<Etching>();
            _designDisplay = GetComponent<DesignDisplay>();
            _designDisplay.design = _etching.design;
            _designDisplay.MaxDescriptionHeight = 85f;

            _normalColor = _designDisplay.TitleText.color;
        }
        
        private void OnDestroy()
        {
            if (BattleRenderer.Current)
                BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.DesignModified || battleEvent.type == BattleEventType.StartedNextPlayerTurn)
            {
                _designDisplay.UpdateDisplay();
            }
            else if ((battleEvent.type == BattleEventType.EtchingActivated) && battleEvent.creatorID == _etching.ResponderID)
            {
                if (battleEvent.showResponse)
                    StartCoroutine(ColorForActivate());
                
                _designDisplay.UpdateDisplay();
            }
        }
        
        private IEnumerator ColorForActivate()
        {
            _designDisplay.TitleText.color = Color.green;
            yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed);
            _designDisplay.TitleText.color = _normalColor;
        }
    }
}