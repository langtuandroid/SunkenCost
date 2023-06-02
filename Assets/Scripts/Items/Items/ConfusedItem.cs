using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class ConfusedItem : ExtraPlankItem
    {
        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                AddResponseTrigger(BattleEventType.StartedBattle, b=> RandomisePlanks()),
                AddResponseTrigger(BattleEventType.EndedEnemyTurn, b=> RandomisePlanks()),
            };
        }

        private BattleEventPackage RandomisePlanks()
        {
            var planksInOrder = Board.Current.PlanksInOrder;
            var etchingsInOrder = planksInOrder.Select(p => p.Etching).ToList();
            var shuffledEtchings = etchingsInOrder.Shuffle().ToList();
            
            // Never get the same sequence
            while (etchingsInOrder.SequenceEqual(shuffledEtchings))
            {
                shuffledEtchings = etchingsInOrder.Shuffle().ToList();
            }
            
            var etchingFactory = EtchingFactory.Current;

            for (var i = 0; i < planksInOrder.Count; i++)
            {
                etchingFactory.MoveEtching(planksInOrder[i], shuffledEtchings[i]);
            }
            
            return new BattleEventPackage(new BattleEvent(BattleEventType.EtchingsOrderChanged));
        }
    }
}