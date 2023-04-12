using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;
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

        protected void ClearResponders()
        {
            _actionCreators.Clear();
        }

        public virtual BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            return _actionCreators.Where(t => t.GetIfRespondingToBattleEvent(previousBattleEvent)).ToArray();
        }

        public DamageModificationPackage GetDamageModifiers(EnemyDamageBattleEvent enemyDamageBattleEvent)
        {
            var flatModifiers = 
                _actionCreators.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(enemyDamageBattleEvent) 
                    select modifier.GetDamageAddition(enemyDamageBattleEvent)).ToList();
            
            var multiModifiers = 
                _actionCreators.OfType<IDamageMultiplierModifier>();
            
            var multiModifications = 
                (from modifier in multiModifiers 
                    where modifier.CanModify(enemyDamageBattleEvent) 
                    select modifier.GetDamageMultiplier(enemyDamageBattleEvent)).ToList();
            
            
            return new DamageModificationPackage(flatModifications, multiModifications);
        }
    }
}