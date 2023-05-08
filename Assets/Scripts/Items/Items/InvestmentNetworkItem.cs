using BattleScreen;
using BattleScreen.BattleEvents;
using Disturbances;

namespace Items.Items
{
    public class InvestmentNetworkItem : EquippedItem, IHasPickupAction
    {
        public void OnPickup()
        {
            DisturbanceLoader.ModifyDisturbanceAsset(DisturbanceType.GoldRush, Amount);
        }

        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return false;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}