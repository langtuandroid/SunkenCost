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
        public PlankDisplay PlankDisplay { get; protected set; }
        protected bool deactivated;

        protected int UsesPerTurn => design.GetStat(StatType.UsesPerTurn);
        protected int PlankNum => PlankDisplay.GetPlankNum();

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        protected void Awake()
        {
            PlankDisplay = transform.parent.parent.GetComponent<PlankDisplay>();
        }

        protected int GetStatValue(StatType statType)
        {
            if (design.Stats.ContainsKey(statType)) return design.Stats[statType].Value;
            return -1;
        }

        public BattleEvent Deactivate(DamageSource source)
        {
            PlankDisplay.SetActiveColor(false);
            deactivated = true;
            return CreateEvent(BattleEventType.PlankDeactivated, source);
        }

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EndedEnemyTurn)
            {
                UsesUsedThisTurn = 0;
                if (deactivated) return true;
            }

            return GetIfDesignIsRespondingToEvent(battleEvent);
        }

        public override List<BattleEvent> GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EndedEnemyTurn && deactivated)
            {
                deactivated = false;
                return new List<BattleEvent>() {new BattleEvent(BattleEventType.EtchingActivated)};
            }

            var response = new List<BattleEvent>();
            var plankActivation = CreateEvent(BattleEventType.EtchingActivated);
            response.AddRange(BattleEventsManager.Current.GetEventAndResponsesList(plankActivation));

            var designResponses = GetDesignResponsesToEvent(battleEvent);
            foreach (var designResponse in designResponses)
            {
                response.AddRange(BattleEventsManager.Current.GetEventAndResponsesList(designResponse));
            }

            UsesUsedThisTurn++;
            return response;
        }

        protected BattleEvent CreateEvent(BattleEventType type, DamageSource source = DamageSource.None, Enemy enemy = null, int modifier = 0)
        {
            return new BattleEvent(type, this) {etching = this, damageSource = source, enemyAffectee = enemy, modifier = modifier};
        }
        
        protected abstract bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent);

        protected abstract List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent);
    }
}
