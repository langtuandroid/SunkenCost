using System.Collections.Generic;
using Disturbances;
using Pickups.Rewards;
using UnityEngine;

namespace Loaders
{
    public class RewardLoader : MonoBehaviour
    {
        private static RewardLoader _current;
        private readonly Dictionary<RewardType, RewardAsset> _rewards = new Dictionary<RewardType, RewardAsset>();

        private void Awake()
        {
            if (_current)
            {
                Destroy(gameObject);
                return;
            }
        
            _current = this;
        }
        
        private void Start()
        {
            LoadRewardAssets();
        }

        public static void LoadRewardAssets()
        {
            _current._rewards.Clear();
            var rewardAssets = Extensions.LoadScriptableObjects<RewardAsset>();

            foreach (var reward in rewardAssets)
            {
                _current._rewards.Add(reward.rewardType, reward);
            }
        }

        public static RewardAsset GetReward(RewardType rewardType)
        {
            return _current._rewards[rewardType];
        }

        public static void ModifyRewardAsset(RewardType rewardType, int amount)
        {
            var reward = GetReward(rewardType);

            reward.ModifyAmount(amount);
        }
    }
}
