using System;
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
            // Works much like the Battle Event Manager's GetNextResponse function, but this only checks each responder
            // Once per battle event
            var index = _indexTracker.GetOrCreateIndex(battleEventToRespondTo);

            while (index < _battleEventResponders.Count)
            {
                var responsePackage = _battleEventResponders[index].GetResponseToBattleEvent(battleEventToRespondTo);
                
                if (!responsePackage.IsEmpty)
                     Debug.Log(_battleEventResponders[index].GetType().Name + " responding to: " 
                          + battleEventToRespondTo.type + " with " + String.Join(", ", 
                              responsePackage.battleEvents.ConvertAll(i => i.type + "(" + i.modifier + ")").ToArray()));
                
                index++;
                _indexTracker.SetIndex(battleEventToRespondTo, index);

                if (responsePackage.IsEmpty) continue;

                return responsePackage;
            }

            return BattleEventPackage.Empty;
        }
        
        public DamageModificationPackage GetDamageModifiers(EnemyDamage damage)
        {
            var flatModifiers = 
                _battleEventResponders.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(damage) 
                    select modifier.GetDamageAddition(damage)).ToList();
            
            var multiModifiers = 
                _battleEventResponders.OfType<IDamageMultiplierModifier>();
            
            var multiModifications = 
                (from modifier in multiModifiers
                    where modifier.CanModify(damage) 
                    select modifier.GetDamageMultiplier(damage)).ToList();
            
            
            return new DamageModificationPackage(flatModifications, multiModifications);
        }

        /*
        public virtual BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            return _actionCreators.Where(t => t.GetIfRespondingToBattleEvent(previousBattleEvent)).ToArray();
        }

        
        
        */
        public virtual void RefreshTransforms()
        {
        }
    }
}