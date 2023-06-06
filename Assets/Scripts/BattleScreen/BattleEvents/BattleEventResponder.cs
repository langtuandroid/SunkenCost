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

        public virtual List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>();
        }

        public virtual List<BattleEventActionTrigger> GetBattleEventActionTriggers()
        {
            return new List<BattleEventActionTrigger>();
        }

        protected BattleEventResponseTrigger PackageResponseTrigger(BattleEventType battleEventType,
            Func<BattleEvent, BattleEventPackage> response, Func<BattleEvent, bool> condition = null)
        {
            return new BattleEventResponseTrigger(battleEventType, response, condition);
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

        protected BattleEventActionTrigger ActionTriggerWithArgument(BattleEventType battleEventType,
            Action<BattleEvent> action, Func<BattleEvent, bool> condition = null)
        {
            return new BattleEventActionTrigger(battleEventType, action, condition);
        }

        protected BattleEventActionTrigger ActionTrigger(BattleEventType battleEventType,
            Action action, Func<BattleEvent, bool> condition = null)
        {
            return new BattleEventActionTrigger(battleEventType, action, condition);
        }

        protected bool GetIfThisIsPrimaryResponder(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.primaryResponderID == ResponderID;
        }
    }
}