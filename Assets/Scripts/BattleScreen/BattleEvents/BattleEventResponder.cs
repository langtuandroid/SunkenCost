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
            return new BattleEventResponseTrigger(battleEventType, ResponderID, response, condition);
        }
        
        protected BattleEventResponseTrigger EventResponseTriggerWithArgument(BattleEventType battleEventType,
            Func<BattleEvent, BattleEvent> response, Func<BattleEvent, bool> condition = null)
        {
            return PackageResponseTrigger(battleEventType, b => new BattleEventPackage(response.Invoke(b)), condition);
        }

        protected BattleEventResponseTrigger EventResponseTrigger(BattleEventType battleEventType,
            Func<BattleEvent> response, Func<BattleEvent, bool> condition = null)
        {
            return PackageResponseTrigger(battleEventType, b => new BattleEventPackage(response.Invoke()), condition);
        }

        protected ActionTrigger ActionTriggerWithArgument(BattleEventType battleEventType,
            Action<BattleEvent> action, Func<BattleEvent, bool> condition = null)
        {
            return new ActionTrigger(battleEventType, ResponderID, action, condition);
        }

        protected ActionTrigger ActionTrigger(BattleEventType battleEventType,
            Action action, Func<BattleEvent, bool> condition = null)
        {
            return new ActionTrigger(battleEventType, ResponderID, action, condition);
        }

        protected bool GetIfThisIsPrimaryResponder(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.primaryResponderID == ResponderID;
        }
    }
}