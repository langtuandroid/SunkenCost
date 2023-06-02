using System;
using System.Linq;
using BattleScreen.BattleBoard;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EtchingBattleEventHandlerGroup : BattleEventHandlerGroup
    {
        private void RefreshEtchingResponderOrder()
        {
            var etchings = Board.Current.PlanksInOrder.Select(p => p.Etching as BattleEventHandler).ToList();
            RefreshHandlers(etchings);
        }

        public override void RefreshTransforms()
        {
            RefreshEtchingResponderOrder();
        }
    }
}