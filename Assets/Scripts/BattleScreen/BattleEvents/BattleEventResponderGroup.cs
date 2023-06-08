using System.Collections.Generic;
using System.Linq;
using Damage;
using UnityEngine;

namespace BattleScreen.BattleEvents
{ 
    public abstract class BattleEventResponderGroup : MonoBehaviour
    {
        private readonly Dictionary<BattleEvent, IEnumerator<BattleEventPackage>> _enumerators =
            new Dictionary<BattleEvent, IEnumerator<BattleEventPackage>>();

        private List<TriggerMap> _triggerMaps = new List<TriggerMap>();

        protected void AddResponder(BattleEventResponder responder)
        {
            _triggerMaps.Add(new TriggerMap
                (responder, responder.GetBattleEventResponseTriggers(), responder.GetBattleEventActionTriggers()));
        }

        protected void RemoveResponder(BattleEventResponder responder)
        {
            _triggerMaps.Remove(_triggerMaps.FirstOrDefault(t => t.responder == responder));
        }

        protected bool HasResponder(BattleEventResponder responder)
        {
            return _triggerMaps.Any(t => t.responder == responder);
        }

        protected void RefreshResponders(List<BattleEventResponder> sortedOrder)
        {
            var respondersToRemove = _triggerMaps.
                Where(r => !sortedOrder.Contains(r.responder)).
                Select(r => r.responder).ToArray();
            
            foreach (var responder in respondersToRemove)
            {
                RemoveResponder(responder);
            }

            foreach (var responder in sortedOrder.Where(r => !HasResponder(r)))
            {
                AddResponder(responder);
            }

            _triggerMaps = _triggerMaps.OrderBy(t => sortedOrder.IndexOf(t.responder)).ToList();
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
            var triggerMaps = new List<TriggerMap>(_triggerMaps);
            for (var i = 0; i < triggerMaps.Count; i++)
            {
                if (triggerMaps[i] is null) continue;

                // Find all the matching response triggers for the event type
                var matchingResponseTriggers = triggerMaps[i].responseTriggers
                    .Where(r => r.battleEventType == previousBattleEvent.type);

                foreach (var responseTrigger in matchingResponseTriggers)
                {
                    // Continue if it doesn't meet the condition
                    if (!responseTrigger.condition.Invoke(previousBattleEvent)) continue;

                    var responsePackage = responseTrigger.response.Invoke(previousBattleEvent);

                    if (responsePackage.IsEmpty) continue;
                    
                    
                    // The next few lines are just for the debug.log - all that's really happening here is the
                    // returning of the responsePackage
                    var responder = _triggerMaps[i].responder.GetType().Name;
                    var eventsString = string.Join(", ",
                        responsePackage.battleEvents.ConvertAll(b => $"{b.type} ({b.modifier})").ToArray());
                    Debug.Log($"{responder} responding to: {previousBattleEvent.type} with {eventsString}");

                    yield return responsePackage;
                }
                
                // Execute all the matching action triggers
                var matchingActionTriggers = triggerMaps[i].actionTriggers.Where(a =>
                    a.battleEventType == previousBattleEvent.type);

                foreach (var battleEventActionTrigger in matchingActionTriggers)
                {
                    if (!battleEventActionTrigger.condition.Invoke(previousBattleEvent)) continue;
                    battleEventActionTrigger.action.Invoke(previousBattleEvent);
                }
            }
        }

        public DamageModificationPackage GetDamageModifiers(EnemyDamage damage)
        {
            var battleEventResponders = _triggerMaps.Select(t => t.responder).ToArray();
            
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
        
        public virtual void RefreshTransforms()
        {
        }
        
        private class TriggerMap
        {
            public readonly BattleEventResponder responder;
            public readonly List<BattleEventResponseTrigger> responseTriggers;
            public readonly List<BattleEventActionTrigger> actionTriggers;

            public TriggerMap(BattleEventResponder responder, List<BattleEventResponseTrigger> responseTriggers,
                List<BattleEventActionTrigger> actionTriggers)
            {
                this.responder = responder;
                this.responseTriggers = responseTriggers;
                this.actionTriggers = actionTriggers;
            }
        }
    }
}