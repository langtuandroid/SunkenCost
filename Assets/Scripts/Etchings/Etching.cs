using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
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

                var designResponseEvents = GetDesignResponsesToEvent(battleEvent);

                var planksToColor = designResponseEvents.Select
                    (designResponseEvent => designResponseEvent.type == BattleEventType.EnemyDamaged
                        ? designResponseEvent.enemyAffectee.PlankNum
                        : PlankNum).ToList();

                var etchingActivatedEvent = CreateEvent(BattleEventType.EtchingActivated);
                etchingActivatedEvent.planksToColor = planksToColor;
                
                var response = new List<BattleEvent>();
                response.Add(etchingActivatedEvent);
                response.AddRange(designResponseEvents);
                return new BattleEventPackage(response.ToArray());
            }
            
            return BattleEventPackage.Empty;
        }

        protected BattleEvent CreateEvent(BattleEventType type, DamageSource source = DamageSource.None, Enemy enemy = null, int modifier = 0)
        {
            return new BattleEvent(type, this) {etching = this, damageSource = source, enemyAffectee = enemy, modifier = modifier};
        }
        
        protected abstract bool GetIfDesignIsRespondingToEvent(BattleEvent battleEvent);

        protected abstract List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent);
    }
}
