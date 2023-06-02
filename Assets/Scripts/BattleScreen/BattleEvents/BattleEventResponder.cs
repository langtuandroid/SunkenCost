using System.Collections.Generic;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public abstract class BattleEventResponder : MonoBehaviour, IBattleEventListener
    {
        public static readonly List<BattleEventResponder> AllBattleEventRespondersByID = new List<BattleEventResponder>();
        private int ResponderID { get; } = AllBattleEventRespondersByID.Count;

        protected virtual void Awake()
        {
            AllBattleEventRespondersByID.Add(this);
        }
        
        public abstract List<BattleEventResponseTrigger> GetResponseTriggers();

        public abstract List<ActionTrigger> GetActionTriggers();
    }
}