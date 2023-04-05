using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Damage;
using UnityEngine;

namespace BattleScreen
{ 
    public abstract class BattleEventResponderGroup : MonoBehaviour
    {
        private readonly List<IBattleEventResponder> _actionCreators = new List<IBattleEventResponder>();

        public void Add(IBattleEventResponder responder)
        {
            _actionCreators.Add(responder);
        }

        public IBattleEventResponder[] GetTurnActionsToExecute(BattleEvent previousBattleEvent)
        {
            return _actionCreators.Where(t => t.GetResponseToBattleEvent(previousBattleEvent)).ToArray();
        }

        public DamageModificationPackage GetDamageModifiers(DamageBattleEvent damageBattleEvent)
        {
            var flatModifiers = 
                _actionCreators.OfType<IDamageFlatModifier>();
            
            var flatModifications = 
                (from modifier in flatModifiers 
                    where modifier.CanModify(damageBattleEvent) 
                    select modifier.GetDamageAddition(damageBattleEvent)).ToList();
            
            var multiModifiers = 
                _actionCreators.OfType<IDamageMultiplierModifier>();
            
            var multiModifications = 
                (from modifier in multiModifiers 
                    where modifier.CanModify(damageBattleEvent) 
                    select modifier.GetDamageMultiplier(damageBattleEvent)).ToList();
            
            
            return new DamageModificationPackage(flatModifications, multiModifications);
        }
    }
}