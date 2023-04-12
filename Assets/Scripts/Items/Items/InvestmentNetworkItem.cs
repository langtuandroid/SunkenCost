using BattleScreen;
using Disturbances;

namespace Items.Items
{
    public class InvestmentNetworkItem : EquippedItem, IHasPickupAction
    {
        public void OnPickup()
        {
            DisturbanceManager.ModifyDisturbanceAsset(DisturbanceType.GoldRush, Amount);
        }

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return false;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}