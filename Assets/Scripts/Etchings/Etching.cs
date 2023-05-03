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

        protected int GetStatValue(StatType statType)
        {
            if (design.Stats.ContainsKey(statType)) return design.Stats[statType].Value;
            return -1;
        }

        private BattleEvent UnStun()
        {
            stunned = false;
            return new BattleEvent(BattleEventType.EtchingUnStunned) {affectedResponderID = ResponderID};
        }

        public BattleEvent Stun(DamageSource source)
        {
            stunned = true;
            return new BattleEvent(BattleEventType.EtchingStunned) {source =  source, affectedResponderID = ResponderID};
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.StartNextPlayerTurn)
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

                var designResponseEvents = GetDesignResponsesToEvent(battleEvent);

                var planksToColor = designResponseEvents.Select
                    (designResponseEvent => designResponseEvent.type == BattleEventType.EnemyAttacked
                        ? BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID).PlankNum
                        : PlankNum);

                var etchingActivatedEvent = new BattleEvent(BattleEventType.EtchingActivated, planksToColor.ToArray())
                {
                    affectedResponderID = ResponderID
                };
                
                var response = new List<BattleEvent>();
                response.AddRange(designResponseEvents);
                response.Add(etchingActivatedEvent);
                return new BattleEventPackage(response.ToArray());
            }
            
            return BattleEventPackage.Empty;
        }
        
        protected abstract bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent);

        protected abstract List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent);
    }
}
