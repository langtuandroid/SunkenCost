using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
using Designs;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etchings
{
    public abstract class Etching : BattleEventResponder
    {
        public Design design;
        protected DesignDisplay designDisplay;
        
        protected bool colorWhenActivated = false;
        
        private Color _normalColor;

        public Plank Plank { get; protected set; }
        protected int deactivationTurns;

        protected int UsesPerTurn => design.GetStat(StatType.UsesPerTurn);

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        protected void Awake()
        {
            Plank = transform.parent.parent.GetComponent<Plank>();
            designDisplay = GetComponent<DesignDisplay>();
        }

        protected virtual void Start()
        {
            designDisplay.design = design;
            _normalColor = designDisplay.TitleText.color;
        }

        protected int GetStatValue(StatType statType)
        {
            if (design.Stats.ContainsKey(statType)) return design.Stats[statType].Value;
            return -1;
        }

        protected abstract bool CheckInfluence(int stickNum);

        private void EndEnemyTurn()
        {
            UsesUsedThisTurn = 0;

            if (deactivationTurns > 0)
            {
                deactivationTurns--;

                if (deactivationTurns == 0)
                {
                    Plank.SetActiveColor(true);
                }
            }
            
            designDisplay.Refresh();
        }
        
        private IEnumerator ColorForActivate()
        {
            designDisplay.TitleText.color = Color.green;
            yield return new WaitForSeconds(BattleManager.AttackTime);
            designDisplay.TitleText.color = _normalColor;
        }

        public IEnumerator Deactivate(int turns)
        {
            Plank.SetActiveColor(false);
            deactivationTurns = turns;
            yield return StartCoroutine(BattleSingleton.Current.BattleEvents.EtchingDeactivated());
        }

        public bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            switch (battleEvent.battleEventType)
            {
                case BattleEventType.EndedEnemyTurn:
                    EndEnemyTurn();
                    break;
                case BattleEventType.PlayerMovedPlank:
                    designDisplay.Refresh();
                    break;
            }

            if (deactivationTurns > 0) return false;
            
            return GetDesignResponseToEvent(battleEvent);
        }

        public IEnumerator ExecuteResponseToAction(BattleEvent battleEvent)
        {
            StartCoroutine(ColorForActivate());
            yield return new WaitForSeconds(BattleEventsManager.ActionExecutionSpeed);
            yield return StartCoroutine(Activate(battleEvent));
        }

        protected abstract bool GetDesignResponseToEvent(BattleEvent battleEvent);

        protected abstract IEnumerator Activate(BattleEvent battleEvent);
    }
}
