using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public abstract class BattleEventResponder : MonoBehaviour
    {
        public static readonly List<BattleEventResponder> AllBattleEventRespondersByID = new List<BattleEventResponder>();
        public int ResponderID { get; } = AllBattleEventRespondersByID.Count;

        protected virtual void Awake()
        {
            AllBattleEventRespondersByID.Add(this);
        }

        public abstract List<BattleEventResponseTrigger> GetBattleEventResponseTriggers();

        protected BattleEventResponseTrigger PackageResponseTrigger(BattleEventType battleEventType,
            Func<BattleEvent, BattleEventPackage> response, Func<BattleEvent, bool> condition = null)
        {
            condition ??= b => true;
            return new BattleEventResponseTrigger(battleEventType, ResponderID, condition, response);
        }
        
        protected BattleEventResponseTrigger EventResponseTrigger(BattleEventType battleEventType,
            Func<BattleEvent, BattleEvent> response, Func<BattleEvent, bool> condition = null)
        {
            return PackageResponseTrigger(battleEventType, e => new BattleEventPackage(response.Invoke(e)), condition);
        }
        
        protected BattleEventResponseTrigger ActionTriggerWithArgument(BattleEventType battleEventType,
            Action<BattleEvent> action, Func<BattleEvent, bool> condition = null)
        {
            return PackageResponseTrigger(battleEventType, 
                e => { action.Invoke(e); return BattleEventPackage.Empty; }, condition);
        }

        protected BattleEventResponseTrigger ActionTrigger(BattleEventType battleEventType,
            Action action, Func<BattleEvent, bool> condition = null)
        {
            return PackageResponseTrigger(battleEventType, 
                e => { action.Invoke(); return BattleEventPackage.Empty; }, condition);
        }

        protected bool GetIfThisIsPrimaryResponder(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.primaryResponderID == ResponderID;
        }
    }
}