using Disturbances;
using Items;
using UnityEngine;

namespace Pickups.Rewards
{
    public class ItemReward : Reward
    {
        public ItemInstance ItemInstance { get; private set; }

        public ItemReward(RewardAsset rewardAsset, int modifier, ItemInstance itemInstance)
            : base(rewardAsset, modifier)
        {
            ItemInstance = itemInstance;
        }

        protected override string GetAdditionalTitle()
        {
            return base.GetDescription() + ItemInstance.Title;
        }

        public override string GetDescription()
        {
            return ItemInstance.Description;
        }
        
        public override Sprite GetSprite()
        {
            return ItemInstance.Sprite;
        }
    }
}