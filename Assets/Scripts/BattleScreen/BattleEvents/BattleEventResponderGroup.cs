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
        private readonly Dictionary<BattleEvent, IEnumerator<BattleEventPackage>> _enumerators =
            new Dictionary<BattleEvent, IEnumerator<BattleEventPackage>>();

        private readonly Dictionary<BattleEventResponder, List<BattleEventResponseTrigger>> _responderAndTriggersDict =
            new Dictionary<BattleEventResponder, List<BattleEventResponseTrigger>>();

        protected void AddResponder(BattleEventResponder responder)
        {
            _responderAndTriggersDict.Add(responder, responder.GetBattleEventResponseTriggers());
        }

        protected void RemoveResponder(BattleEventResponder responder)
        {
            _responderAndTriggersDict.Remove(responder);
        }

        protected void ClearResponders()
        {
            _responderAndTriggersDict.Clear();
        }
        
        protected bool HasResponder(BattleEventResponder responder)
        {
            return _responderAndTriggersDict.ContainsKey(responder);
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
            foreach (var (battleEventResponder, responseTriggers) in _responderAndTriggersDict)
            {
                // Find all the matching triggers for the event type
                var matchingResponseTriggers = responseTriggers
                    .Where(r => r.battleEventType == previousBattleEvent.type).ToArray();

                foreach (var responseTrigger in matchingResponseTriggers)
                {
                    // Continue if it doesn't meet the condition
                    if (!responseTrigger.condition.Invoke(previousBattleEvent)) continue;

                    var responsePackage = responseTrigger.response.Invoke(previousBattleEvent);

                    if (responsePackage.IsEmpty) continue;
                    
                    
                    // The next few lines are just for the debug.log - all that's really happening here is the
                    // returning of the responsePackage
                    var responder = battleEventResponder.GetType().Name;
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