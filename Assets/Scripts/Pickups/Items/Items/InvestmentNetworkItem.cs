using BattleScreen;
using BattleScreen.BattleEvents;
using Disturbances;
using Loaders;
using Pickups.Rewards;

namespace Items.Items
{
    public class InvestmentNetworkItem : EquippedItem, IHasPickupAction
    {
        public void OnPickup()
        {
            RewardLoader.ModifyRewardAsset(RewardType.GoldRush, Amount);
        }
    }
}