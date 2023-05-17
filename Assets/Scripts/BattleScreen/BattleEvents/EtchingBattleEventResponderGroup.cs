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
            ClearResponders();
            var etchings = Board.Current.PlanksInOrder.Select(p => p.Etching);
            Debug.Log(etchings.Count());
            foreach (var responder in etchings.Select(etching => etching as BattleEventResponder))
            {
                if (responder is null) throw new Exception("Etching is not responder!");
                AddResponder(responder);
            }
        }

        public override void RefreshTransforms()
        {
            RefreshEtchingResponderOrder();
        }
    }
}