using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;
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
        public int PlankNum => _plank.PlankNum;

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _plank = GetComponentInParent<Plank>();
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EndedEnemyTurn)
            {
                UsesUsedThisTurn = 0;

                if (stunned)
                {
                    stunned = false;
                    return new BattleEventPackage(UnStun());
                } 
            }
            
            if (GetIfDesignIsRespondingToEvent(battleEvent))
            {
                UsesUsedThisTurn++;

                var designResponse = GetDesignResponsesToEvent(battleEvent);
                var planksToColor = designResponse.planksToColor;
                if (planksToColor.Contains(-1)) planksToColor = new int[0];
                var etchingActivatedEvent = new BattleEvent(BattleEventType.EtchingActivated, planksToColor)
                {
                    primaryResponderID = ResponderID,
                    showResponse = designResponse.showResponse
                };

                var response = new List<BattleEvent>(designResponse.response) {etchingActivatedEvent};
                return new BattleEventPackage(response);
            }
            
            return BattleEventPackage.Empty;
        }
        
        public BattleEvent Stun(DamageSource source)
        {
            stunned = true;
            return new BattleEvent(BattleEventType.EtchingStunned) {source =  source, primaryResponderID = ResponderID};
        }

        public BattleEvent AddStatModifier(StatType statType, StatModifier mod)
        {
            design.AddStatModifier(statType, mod);
            return new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
        }
        
        public BattleEvent RemoveStatModifier(StatType statType, StatModifier mod)
        {
            design.RemoveStatModifier(statType, mod);
            return new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
        }
        
        protected abstract bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent);

        protected abstract DesignResponse GetDesignResponsesToEvent(BattleEvent battleEvent);

        private BattleEvent UnStun()
        {
            stunned = false;
            return new BattleEvent(BattleEventType.EtchingUnStunned) {primaryResponderID = ResponderID};
        }

        protected readonly struct DesignResponse
        {
            public readonly int[] planksToColor;
            public readonly List<BattleEvent> response;
            public readonly bool showResponse;

            public DesignResponse(List<int> planksToColor, List<BattleEvent> response, bool showResponse = true)
            {
                this.planksToColor = planksToColor.ToArray();
                this.response = response;
                this.showResponse = showResponse;
            }

            public DesignResponse(int plankToColor, List<BattleEvent> response, bool showResponse = true) : this(new List<int> {plankToColor},
                response, showResponse)
            {
            }

            public DesignResponse(int plankToColor, BattleEvent response, bool showResponse = true) : this(new List<int> {plankToColor},
                new List<BattleEvent> {response}, showResponse)
            {
            }
        }
    }
}
