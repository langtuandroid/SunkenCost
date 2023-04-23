using System;
using System.Linq;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Etchings;
using UnityEngine;

namespace BattleScreen.Events
{
    public class EtchingBattleEventResponderGroup : BattleEventResponderGroup
    {
        public override BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            if (previousBattleEvent.type == BattleEventType.StartedBattle ||
                previousBattleEvent.type == BattleEventType.PlayerMovedPlank ||
                previousBattleEvent.type == BattleEventType.PlankCreated ||
                previousBattleEvent.type == BattleEventType.PlankDestroyed ||
                previousBattleEvent.type == BattleEventType.StartedEnemyTurn)
            {
                
                RefreshEtchingResponderOrder();
            }

            return base.GetEventResponders(previousBattleEvent);
        }

        private void RefreshEtchingResponderOrder()
        {
            ClearResponders();
            var etchings = Board.Current.PlanksInOrder.Select(p => p.Etching);
            foreach (var responder in etchings.Select(etching => etching as BattleEventResponder))
            {
                if (responder is null) throw new Exception("Etching is not responder!");
                AddResponder(responder);
            }
        }
    }
}