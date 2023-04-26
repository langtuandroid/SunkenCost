using System.Collections;
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
            return battleEvent.type == BattleEventType.StartedBattle;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            Board.Current.RandomisePlanks();
            // TODO: Change this to "Item moved plank"
            return new BattleEventPackage(new BattleEvent(BattleEventType.PlankMoved));
        }
    }
}