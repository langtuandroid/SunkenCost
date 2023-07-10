using System.Collections;
using System.Collections.Generic;
using MapScreen;
using Pickups;
using Pickups.Rewards;
using UnityEngine;
using UnityEngine.Serialization;

namespace Disturbances
{
    [CreateAssetMenu(menuName = "Reward")]
    public class RewardAsset : PickupAsset
    {
        [FormerlySerializedAs("disturbanceType")] public RewardType rewardType;
        
        public void ModifyAmount(int modifyAmount)
        {
            modifier += modifyAmount;
        }
    }
}