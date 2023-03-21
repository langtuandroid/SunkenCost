using Disturbances;

namespace Items.Items
{
    public class InvestmentNetworkItem : EquippedItem, IHasPickupAction
    {
        public void OnPickup()
        {
            DisturbanceManager.ModifyDisturbanceAsset(DisturbanceType.GoldRush, Amount);
        }
    }
}