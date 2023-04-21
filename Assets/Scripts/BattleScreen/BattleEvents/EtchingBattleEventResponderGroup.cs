using System;
using System.Linq;
using BattleScreen.BattleEvents;

namespace BattleScreen.Events
{
    public class EtchingBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void Start()
        {
            RefreshEtchingResponderOrder();
        }

        public override BattleEventResponder[] GetEventResponders(BattleEvent previousBattleEvent)
        {
            if (previousBattleEvent.type == BattleEventType.PlayerMovedPlank ||
                previousBattleEvent.type == BattleEventType.PlankCreated ||
                previousBattleEvent.type == BattleEventType.PlankDestroyed ||
                previousBattleEvent.type == BattleEventType.StartedEnemyTurn)
            {
                EtchingMap.Current.RefreshEtchingOrder();
                RefreshEtchingResponderOrder();
            }

            return base.GetEventResponders(previousBattleEvent);
        }

        private void RefreshEtchingResponderOrder()
        {
            ClearResponders();
            var etchings = EtchingMap.Current.EtchingOrder;
            foreach (var responder in etchings.Select(etching => etching as BattleEventResponder))
            {
                if (responder is null) throw new Exception("Etching is not responder!");
                AddResponder(responder);
            }
        }
    }
}