using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using Damage;
using UnityEngine;

namespace BattleScreen
{ 
    public abstract class BattleEventResponderGroup : MonoBehaviour
    {
        private readonly List<BattleEventResponder> _battleEventResponders = new List<BattleEventResponder>();

        private readonly BattleEventResponderTracker _indexTracker = new BattleEventResponderTracker();

        protected void AddResponder(BattleEventResponder responder)
        {
            _battleEventResponders.Add(responder);
        }

        protected void RemoveResponder(BattleEventResponder responder)
        {
            _battleEventResponders.Remove(responder);
        }

        protected void ClearResponders()
        {
            _battleEventResponders.Clear();
        }
        
        protected bool HasResponder(BattleEventResponder responder)
        {
            return _battleEventResponders.Contains(responder);
        }

        public virtual BattleEventPackage GetNextResponse(BattleEvent battleEventToRespondTo)
        {
            var index = _indexTracker.GetIndex(battleEventToRespondTo);

            var responsePackage = BattleEventPackage.Empty;
            for (; index < _battleEventResponders.Count; index++)
            {
                var nextResponder = _battleEventResponders[index];
                if (!nextResponder) continue;
                
                responsePackage = nextResponder.GetResponseToBattleEvent(battleEventToRespondTo);
                if (!responsePackage.IsEmpty) break;
            }
            
            _indexTracker.SetIndex(battleEventToRespondTo, index);
            Debug.Log(GetType().Name + " " + index + "/" + _battleEventResponders.Count + " responding to: " + battleEventToRespondTo.type + " with " + responsePackage.battleEvents[0].type);
            return responsePackage;
        }
        
        public DamageModificationPackage GetDamageModifiers(BattleEvent battleEvent)
        {
            var flatModifiers = 
                _battleEventResponders.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(battleEvent) 
                    select modifier.GetDamageAddition(battleEvent)).ToList();
            
            var multiModifiers = 
                _battleEventResponders.OfType<IDamageMultiplierModifier>();
            
            var multiModifications = 
                (from modifier in multiModifiers
                    where modifier.CanModify(battleEvent) 
                    select modifier.GetDamageMultiplier(battleEvent)).ToList();
            
            
            return new DamageModificationPackage(flatModifications, multiModifications);
        }

        /*
        public virtual BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            return _actionCreators.Where(t => t.GetIfRespondingToBattleEvent(previousBattleEvent)).ToArray();
        }

        
        
        */
    }
}