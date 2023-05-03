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

        public abstract BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent);
    }
}