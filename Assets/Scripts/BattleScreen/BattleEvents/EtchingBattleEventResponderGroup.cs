using System;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleBoard;
using Etchings;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EtchingBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void RefreshEtchingResponderOrder()
        {
            var etchings = Board.Current.PlanksInOrder.Select(p => p.Etching as BattleEventResponder).ToList();
            Debug.Log(string.Join(", ", etchings.Select(e => e.GetType())));
            RefreshResponders(etchings);
        }

        public override void RefreshTransforms()
        {
            RefreshEtchingResponderOrder();
        }
    }
}