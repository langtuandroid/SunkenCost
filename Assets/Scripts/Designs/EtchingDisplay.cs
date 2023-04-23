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
        
                
        private Queue<DesignDisplayState> _queuedStates = new Queue<DesignDisplayState>();

        protected override void Start()
        {
            BattleEventsManager.Current.RegisterUIUpdater(this);
            
            design = GetComponent<Etching>().design;
            base.Start();
        }
        
        private IEnumerator ColorForActivate()
        {
            TitleText.color = Color.green;
            yield return new WaitForSeconds(Battle.ActionExecutionSpeed);
            TitleText.color = _normalColor;
        }

        protected override void Init()
        {
            _normalColor = TitleText.color;
            base.Init();
        }

        public bool GetIfUpdating(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.DesignModified ||
                   battleEvent.type == BattleEventType.EtchingActivated;
        }

        public void SaveCurrentState()
        {
            
            _queuedStates.Enqueue(GetDisplayState());
        }

        public void LoadNextState()
        {
            UpdateDisplay(_queuedStates.Dequeue());
        }

        private void OnDestroy()
        {
            BattleEventsManager.Current.DeregisterUIUpdater(this);
        }
    }
}