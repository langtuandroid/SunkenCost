using System.Collections;
using BattleScreen;
using BattleScreen.BattleBoard;

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

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.type == BattleEventType.StartedBattle;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            Board.Current.RandomisePlanks();
            return BattleEvent.None;
        }
    }
}