using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleBoard;
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
        protected bool stunned;

        private Plank _plank;

        protected int UsesPerTurn => design.GetStat(StatType.UsesPerTurn);
        protected int PlankNum => Board.Current.PlanksInOrder.IndexOf(_plank);

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        public void Awake()
        {
            _plank = GetComponentInParent<Plank>();
        }

        protected int GetStatValue(StatType statType)
        {
            if (design.Stats.ContainsKey(statType)) return design.Stats[statType].Value;
            return -1;
        }

        private BattleEvent UnStun()
        {
            stunned = false;
            return CreateEvent(BattleEventType.EtchingUnStunned);
        }

        public BattleEvent Stun(DamageSource source)
        {
            stunned = true;
            return CreateEvent(BattleEventType.EtchingStunned, source);
        }

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EndedEnemyTurn)
            {
                UsesUsedThisTurn = 0;
                if (stunned) return true;
            }

            return GetIfDesignIsRespondingToEvent(battleEvent);
        }

        public override List<BattleEvent> GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EndedEnemyTurn && stunned)
            {
                stunned = false;
                return new List<BattleEvent>() {UnStun()};
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
