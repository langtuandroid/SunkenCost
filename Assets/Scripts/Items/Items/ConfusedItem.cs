using System.Collections;
using BattleScreen;

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
            return battleEvent.Type == BattleEventType.StartBattle;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            PlankMap.Current.RandomisePlanks();
            return new BattleEvent(BattleEventType.None);
        }
    }
}