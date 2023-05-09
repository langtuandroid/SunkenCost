using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class MeasuredTouchItem : EquippedItem, IHasPickupAction
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

        protected override bool GetIfRespondingToBattleEvent(BattleEvent previousBattleEvent)
        {
            throw new System.NotImplementedException();
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}