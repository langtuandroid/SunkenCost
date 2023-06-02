using System.Collections.Generic;
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

        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                PackageResponseTrigger(BattleEventType.PlayerMovedPlank, b => new BattleEventPackage(BattleEvent.None))
            };
        }
    }
}