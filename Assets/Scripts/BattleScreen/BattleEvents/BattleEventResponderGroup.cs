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
        private readonly List<BattleEventResponder> _actionCreators = new List<BattleEventResponder>();

        protected void AddResponder(BattleEventResponder responder)
        {
            _actionCreators.Add(responder);
        }

        protected void RemoveResponder(BattleEventResponder responder)
        {
            _actionCreators.Remove(responder);
        }

        protected void ClearResponders()
        {
            _actionCreators.Clear();
        }

        public virtual BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            return _actionCreators.Where(t => t.GetIfRespondingToBattleEvent(previousBattleEvent)).ToArray();
        }

        public DamageModificationPackage GetDamageModifiers(BattleEvent battleEvent)
        {
            var flatModifiers = 
                _actionCreators.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(battleEvent) 
                    select modifier.GetDamageAddition(battleEvent)).ToList();
            
            var multiModifiers = 
                _actionCreators.OfType<IDamageMultiplierModifier>();
            
            var multiModifications = 
                (from modifier in multiModifiers
                    where modifier.CanModify(battleEvent) 
                    select modifier.GetDamageMultiplier(battleEvent)).ToList();
            
            
            return new DamageModificationPackage(flatModifications, multiModifications);
        }
    }
}