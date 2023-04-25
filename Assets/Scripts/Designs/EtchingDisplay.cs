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
        
        private Queue<EtchingDisplayState> _queuedStates = new Queue<EtchingDisplayState>();

        protected override void Start()
        {
            BattleEventsManager.Current.RegisterUIUpdater(this);
            _etching = GetComponent<Etching>();
            design = _etching.design;
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
            return (battleEvent.type == BattleEventType.DesignModified ||
                    battleEvent.type == BattleEventType.EtchingActivated) && battleEvent.etching == _etching;
        }

        public void SaveStateResponse(BattleEventType battleEventType)
        {
            var etchingDisplayState = new EtchingDisplayState(battleEventType, GetDisplayState());
            _queuedStates.Enqueue(etchingDisplayState);
        }

        public void LoadNextState()
        {
            var state = _queuedStates.Dequeue();
            if (state.battleEventType == BattleEventType.EtchingActivated) StartCoroutine(ColorForActivate());
            UpdateDisplay(state.designDisplayState);
        }

        private void OnDestroy()
        {
            BattleEventsManager.Current.DeregisterUIUpdater(this);
        }

        private readonly struct EtchingDisplayState
        {
            public readonly BattleEventType battleEventType;
            public readonly DesignDisplayState designDisplayState;

            public EtchingDisplayState(BattleEventType battleEventType, DesignDisplayState designDisplayState)
            {
                this.battleEventType = battleEventType;
                this.designDisplayState = designDisplayState;
            }
        }
    }
}