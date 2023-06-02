using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class BattleEventHandler
    {
        public IBattleEventListener Listener { get; }

        private readonly List<BattleEventResponseTrigger> _responseTriggers;
        private readonly List<ActionTrigger> _actionTriggers;

        public BattleEventHandler(IBattleEventListener listener)
        {
            Listener = listener;
            _responseTriggers = Listener.GetResponseTriggers();
            _actionTriggers = Listener.GetActionTriggers();
        }

        public List<BattleEventResponseTrigger> GetTriggers()
        {
            return new List<BattleEventResponseTrigger>(_responseTriggers.Concat(_actionTriggers));
        }
    }
}