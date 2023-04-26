using System;
using System.Linq;
using BattleScreen.BattleBoard;

namespace BattleScreen.BattleEvents
{
    public class EtchingBattleEventResponderGroup : BattleEventResponderGroup
    {

        public override BattleEventPackage GetNextResponse(BattleEvent battleEventToRespondTo)
        {
            if (battleEventToRespondTo.type == BattleEventType.StartedBattle ||
                battleEventToRespondTo.type == BattleEventType.PlankMoved ||
                battleEventToRespondTo.type == BattleEventType.PlankCreated ||
                battleEventToRespondTo.type == BattleEventType.PlankDestroyed ||
                battleEventToRespondTo.type == BattleEventType.StartedNextTurn)
            {
                RefreshEtchingResponderOrder();
            }
            
            return base.GetNextResponse(battleEventToRespondTo);
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