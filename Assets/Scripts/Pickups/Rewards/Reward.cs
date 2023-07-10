using Disturbances;
using UnityEngine;

namespace Pickups.Rewards
{
    public class Reward
    {
        private readonly RewardAsset _rewardAsset;
        public RewardType RewardType => _rewardAsset.rewardType;
        
        public int Modifier { get; private set; }
        public bool IsElite => (int)RewardType >= 1000;

        public Reward(RewardAsset rewardAsset, int modifier)
        {
            _rewardAsset = rewardAsset;
            Modifier = modifier;
        }

        public string GetTitle()
        {
            return _rewardAsset.title + "  " + GetAdditionalTitle();
        }

        protected virtual string GetAdditionalTitle()
        {
            return "";
        }

        public virtual string GetDescription()
        {
            return _rewardAsset.GetDescription(_rewardAsset.modifier);
        }

        public virtual Sprite GetSprite()
        {
            return _rewardAsset.sprite;
        }
    }
}