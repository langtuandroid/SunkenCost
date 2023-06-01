using System;
using System.Linq;
using BattleScreen.BattleBoard;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EtchingBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void RefreshEtchingResponderOrder()
        {
            var etchings = Board.Current.PlanksInOrder.Select(p => p.Etching as BattleEventResponder).ToList();
            RefreshResponders(etchings);
        }

        public override void RefreshTransforms()
        {
            RefreshEtchingResponderOrder();
        }
    }
}