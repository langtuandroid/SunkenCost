using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Etchings;
using UnityEngine;

namespace Designs
{
    public class EtchingDisplay : DesignDisplay, IBattleEventUpdatedUI
    {
        protected bool colorWhenActivated = false;
        private Color _normalColor;

        private Etching _etching;

        protected override void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
            _etching = GetComponent<Etching>();
            design = _etching.design;
            base.Start();
        }

        protected override void Init()
        {
            _normalColor = TitleText.color;
            base.Init();
        }

        private void OnDestroy()
        {
            BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if ((battleEvent.type == BattleEventType.DesignModified ||
                 battleEvent.type == BattleEventType.EtchingActivated) && battleEvent.etching == _etching)
            {
                UpdateDisplay();
            }
        }
        
        private IEnumerator ColorForActivate()
        {
            TitleText.color = Color.green;
            yield return new WaitForSeconds(Battle.ActionExecutionSpeed);
            TitleText.color = _normalColor;
        }
    }
}