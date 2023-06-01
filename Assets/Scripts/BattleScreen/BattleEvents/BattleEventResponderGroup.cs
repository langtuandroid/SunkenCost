using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using Damage;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleScreen
{ 
    public abstract class BattleEventResponderGroup : MonoBehaviour
    {
        private readonly Dictionary<BattleEvent, IEnumerator<BattleEventPackage>> _enumerators =
            new Dictionary<BattleEvent, IEnumerator<BattleEventPackage>>();

        private Dictionary<BattleEventResponder, List<BattleEventResponseTrigger>> _responderAndTriggersDict =
            new Dictionary<BattleEventResponder, List<BattleEventResponseTrigger>>();

        protected void AddResponder(BattleEventResponder responder)
        {
            _responderAndTriggersDict.Add(responder, responder.GetBattleEventResponseTriggers());
        }

        protected void RemoveResponder(BattleEventResponder responder)
        {
            _responderAndTriggersDict.Remove(responder);
        }

        protected bool HasResponder(BattleEventResponder responder)
        {
            return _responderAndTriggersDict.ContainsKey(responder);
        }

        protected void RefreshResponders(List<BattleEventResponder> sortedOrder)
        {
            var respondersToRemove = _responderAndTriggersDict.
                Where(r => !sortedOrder.Contains(r.Key)).
                Select(r => r.Key);
            
            foreach (var responder in respondersToRemove)
            {
                RemoveResponder(responder);
            }

            foreach (var responder in sortedOrder.Where(r => !HasResponder(r)))
            {
                AddResponder(responder);
            }

            _responderAndTriggersDict = new Dictionary<BattleEventResponder, List<BattleEventResponseTrigger>>(
                _responderAndTriggersDict.OrderBy(kvp => sortedOrder.IndexOf(kvp.Key)));
        }

        public virtual BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            if (!_enumerators.ContainsKey(previousBattleEvent))
            {
                _enumerators.Add(previousBattleEvent, GetNextPackage(previousBattleEvent));
            }
            
            var enumerator = _enumerators[previousBattleEvent];
            return enumerator.MoveNext() ? enumerator.Current : BattleEventPackage.Empty;
        }

        private IEnumerator<BattleEventPackage> GetNextPackage(BattleEvent previousBattleEvent)
        {
            for (var i = 0; i < _responderAndTriggersDict.Count; i++)
            {
                // Find all the matching triggers for the event type
                var matchingResponseTriggers = _responderAndTriggersDict.ElementAt(i).Value
                    .Where(r => r.battleEventType == previousBattleEvent.type).ToArray();

                foreach (var responseTrigger in matchingResponseTriggers)
                {
                    Assert.IsNotNull(responseTrigger);
                    Assert.IsNotNull(previousBattleEvent);
                    Assert.IsNotNull(responseTrigger.condition);
                    
                    // Continue if it doesn't meet the condition
                    if (!responseTrigger.condition.Invoke(previousBattleEvent)) continue;

                    var responsePackage = responseTrigger.response.Invoke(previousBattleEvent);

                    if (responsePackage.IsEmpty) continue;
                    
                    
                    // The next few lines are just for the debug.log - all that's really happening here is the
                    // returning of the responsePackage
                    var responder = _responderAndTriggersDict.ElementAt(i).Key.GetType().Name;
                    var eventsString = string.Join(", ",
                        responsePackage.battleEvents.ConvertAll(b => $"{b.type} ({b.modifier})").ToArray());
                    Debug.Log($"{responder.GetType().Name} responding to: {previousBattleEvent.type} with {eventsString}");

                    yield return responsePackage;
                }
            }
        }

        public DamageModificationPackage GetDamageModifiers(EnemyDamage damage)
        {
            var battleEventResponders = _responderAndTriggersDict.Select(kvp => kvp.Key).ToArray();
            
            var flatModifiers = 
                battleEventResponders.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(damage) 
                    select modifier.GetDamageAddition(damage)).ToList();
            
            var multiModifiers = 
                battleEventResponders.OfType<IDamageMultiplierModifier>();
            
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