using Designs;
using Disturbances;
using UnityEngine;

namespace Pickups.Rewards
{
    public class CardReward : Reward
    {
        public Design Design { get; private set; }
        
        public CardReward(RewardAsset rewardAsset, int modifier, Design design) 
            : base(rewardAsset, modifier)
        {
            Design = design;
        }
        
        protected override string GetAdditionalTitle()
        {
            return base.GetDescription() + Design.Title;
        }

        public override string GetDescription()
        {
            return Design.designAsset.GetDescription(Design);
        }
        
        public override Sprite GetSprite()
        {
            return Design.Sprite;
        }
    }
}