using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class ConfusedItem : EquippedItem, IHasPickupAction
    {
        private StatModifier _extraPlank;

        public void OnPickup()
        {
            _extraPlank = new StatModifier(Amount, StatModType.Flat);
            RunProgress.PlayerStats.maxPlanks.AddModifier(_extraPlank);
        }
        
        public void OnDestroy()
        {
            RunProgress.PlayerStats.maxPlanks.RemoveModifier(_extraPlank);
        }

        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.EndedEnemyTurn || battleEvent.type == BattleEventType.StartedBattle;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            var planksInOrder = Board.Current.PlanksInOrder;
            var etchingsInOrder = planksInOrder.Select(p => p.Etching);
            var shuffledEtchings = etchingsInOrder.Shuffle().ToList();

            var etchingFactory = EtchingFactory.Current;
            var responses = new List<BattleEvent>();

            for (var i = 0; i < planksInOrder.Count; i++)
            {
                etchingFactory.MoveEtching(planksInOrder[i], shuffledEtchings[i]);
                
                responses.Add(new BattleEvent(BattleEventType.EtchingMoved) {modifier = i});
            }
            
            return new BattleEventPackage(responses);
        }
    }
}