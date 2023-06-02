using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using Damage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleScreen
{ 
    public abstract class BattleEventHandlerGroup : MonoBehaviour
    {
        private readonly Dictionary<BattleEvent, IEnumerator<BattleEventPackage>> _enumerators =
            new Dictionary<BattleEvent, IEnumerator<BattleEventPackage>>();

        private Dictionary<BattleEventHandler, List<BattleEventResponseTrigger>> _handlersAndListenersDict =
            new Dictionary<BattleEventHandler, List<BattleEventResponseTrigger>>();

        public virtual BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            if (!_enumerators.ContainsKey(previousBattleEvent))
            {
                _enumerators.Add(previousBattleEvent, GetNextPackage(previousBattleEvent));
            }
            
            var enumerator = _enumerators[previousBattleEvent];
            return enumerator.MoveNext() ? enumerator.Current : BattleEventPackage.Empty;
        }
        
        public DamageModificationPackage GetDamageModifiers(EnemyDamage damage)
        {
            var battleEventResponders = _handlersAndListenersDict.Select(kvp => kvp.Key).ToArray();
            
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
        
        protected void CreateHandler(IBattleEventListener listener)
        {
            var handler = new BattleEventHandler(listener);

            _handlersAndListenersDict.Add(handler, handler.GetTriggers());
        }

        protected void RemoveHandler(BattleEventHandler handler)
        {
            _handlersAndListenersDict.Remove(handler);
        }


        protected void RefreshHandlers(List<IBattleEventListener> sortedOrder)
        {
            var respondersToRemove = _handlersAndListenersDict.
                Where(r => !sortedOrder.Contains(r.Key.Listener)).
                Select(r => r.Key);
            
            foreach (var responder in respondersToRemove)
            {
                RemoveHandler(responder);
            }

            foreach (var responder in sortedOrder.Where(r => !HasHandler(r)))
            {
                CreateHandler(responder);
            }

            _handlersAndListenersDict = new Dictionary<BattleEventHandler, List<BattleEventResponseTrigger>>(
                _handlersAndListenersDict.OrderBy(kvp => sortedOrder.IndexOf(kvp.Key.Listener)));
        }
        
        private IEnumerator<BattleEventPackage> GetNextPackage(BattleEvent previousBattleEvent)
        {
            for (var i = 0; i < _handlersAndListenersDict.Count; i++)
            {
                // Find all the matching triggers for the event type
                var matchingResponseTriggers = _handlersAndListenersDict.ElementAt(i).Value
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
                    var responder = _handlersAndListenersDict.ElementAt(i).Key.GetType().Name;
                    var eventsString = string.Join(", ",
                        responsePackage.battleEvents.ConvertAll(b => $"{b.type} ({b.modifier})").ToArray());
                    Debug.Log($"{responder.GetType().Name} responding to: {previousBattleEvent.type} with {eventsString}");

                    yield return responsePackage;
                }
            }
        }
        
        [CanBeNull]
        private BattleEventHandler GetHandler(IBattleEventListener listener)
        {
            return _handlersAndListenersDict.Select(kvp => kvp.Key).
                FirstOrDefault(h => h.Listener == listener);
        }

        private bool HasHandler(IBattleEventListener listener)
        {
            return GetHandler(listener) is not null;
        }

    }
}