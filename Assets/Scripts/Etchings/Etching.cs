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
using Pickups.Varnishes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etchings
{
    public abstract class Etching : BattleEventResponder
    {
        protected bool stunned;

        private Plank _plank;

        private readonly Dictionary<BattleEventType, Func<BattleEvent, DesignResponse>> _designResponses =
            new Dictionary<BattleEventType, Func<BattleEvent, DesignResponse>>();
        
        public Design Design { get; private set; }
        protected int UsesPerTurn => Design.GetStat(StatType.UsesPerTurn);
        public int PlankNum => _plank.PlankNum;

        protected int UsesUsedThisTurn
        {
            get => Design.UsesUsedThisTurn;
            set => Design.UsesUsedThisTurn = value;
        }

        protected override void Awake()
        {
            SetPlank(GetComponentInParent<Plank>());
            base.Awake();
        }

        public void SetDesign(Design design)
        {
            Design = design;
        }

        public void SetPlank(Plank plank)
        {
            _plank = plank;
            plank.SetEtching(this);
        }

        public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
        {
            var responseTriggers = new List<BattleEventResponseTrigger>
            {
                PackageResponseTrigger(BattleEventType.EndedEnemyTurn, e => ResetForStartOfTurn())
            };

            var designResponseTriggers = GetDesignResponseTriggers();

            foreach (var designResponseTrigger in designResponseTriggers)
            {
                _designResponses.Add(designResponseTrigger.battleEventType, designResponseTrigger.response);
                responseTriggers.Add(PackageResponseTrigger(designResponseTrigger.battleEventType, 
                    GetDesignResponse, designResponseTrigger.condition));
            }
            
            responseTriggers.AddRange(GetDesignActionTriggers());

            var varnishes = GetComponents<Varnish>();
            foreach (var varnish in varnishes)
            {
                responseTriggers.AddRange(varnish.GetBattleEventResponseTriggers());
            }
            
            return responseTriggers;
        }
        public BattleEvent Stun(DamageSource source)
        {
            stunned = true;
            return new BattleEvent(BattleEventType.EtchingStunned) {source =  source, primaryResponderID = ResponderID};
        }

        public bool HasStat(StatType statType) => Design.HasStat(statType);

        public BattleEvent AddStatModifier(StatType statType, StatModifier mod)
        {
            Design.AddStatModifier(statType, mod);
            return new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
        }
        
        public BattleEvent RemoveStatModifier(StatType statType, StatModifier mod)
        {
            Design.RemoveStatModifier(statType, mod);
            return new BattleEvent(BattleEventType.DesignModified) {primaryResponderID = ResponderID};
        }

        protected abstract List<DesignResponseTrigger> GetDesignResponseTriggers();
        
        protected virtual List<ActionTrigger> GetDesignActionTriggers()
        {
            return new List<ActionTrigger>();
        }
        
        private BattleEventPackage GetDesignResponse(BattleEvent previousBattleEvent)
        {
            UsesUsedThisTurn++;
            
            var designResponse = _designResponses[previousBattleEvent.type].Invoke(previousBattleEvent);
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

        private BattleEventPackage ResetForStartOfTurn()
        {
            UsesUsedThisTurn = 0;

            if (stunned)
            {
                stunned = false;
                return new BattleEventPackage(UnStun());
            } 
            
            return BattleEventPackage.Empty;
        }


        private BattleEvent UnStun()
        {
            stunned = false;
            return new BattleEvent(BattleEventType.EtchingUnStunned) {primaryResponderID = ResponderID};
        }

        protected readonly struct DesignResponseTrigger
        {
            public readonly BattleEventType battleEventType;
            public readonly Func<BattleEvent, DesignResponse> response;
            public readonly Func<BattleEvent, bool> condition;

            public DesignResponseTrigger(BattleEventType battleEventType, Func<BattleEvent, DesignResponse> response, 
                Func<BattleEvent, bool> condition = null)
            {
                this.battleEventType = battleEventType;
                this.response = response;
                condition ??= b => true;
                this.condition = condition;
            }
        }

        protected readonly struct DesignResponse
        {
            public readonly int[] planksToColor;
            public readonly List<BattleEvent> response;
            public readonly bool showResponse;

            public bool IsEmpty => response[0].type == BattleEventType.None;

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
            
            public static DesignResponse Empty => new DesignResponse(-1, BattleEvent.None);
        }
    }
}
