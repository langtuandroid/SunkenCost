using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class MeasuredTouchItem : ExtraPlankItem
    {
        public override void OnPickup()
        {
            RunProgress.Current.PlayerStats.MovesPerTurn = 1;
            base.OnPickup();
        }
        
        public override void OnDestroy()
        {
            RunProgress.Current.PlayerStats.MovesPerTurn = -1;
            base.OnDestroy();
        }

        protected override bool GetIfRespondingToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.type == BattleEventType.PlayerMovedPlank;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(BattleEvent.None);
        }
    }
}