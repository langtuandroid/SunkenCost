using System.Collections.Generic;
using System.Linq;
using Designs;
using Disturbances;
using Items;
using Loaders;
using MapScreen;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups.Rewards
{
    public class RewardGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _rewardOfferPrefab;
        [SerializeField] private RectTransform _rewardOfferGrid;
    
        private static readonly Dictionary<RewardType, float> NormalWeightings = new Dictionary<RewardType, float>()
        {
            {RewardType.GoldRush, 0.5f},
            {RewardType.Heart, 0.2f},
            {RewardType.Card, 0.15f},
            {RewardType.Item, 0.15f}
        };
    
        private static readonly Dictionary<RewardType, float> EliteWeightings = new Dictionary<RewardType, float>()
        {
            {RewardType.EliteItem, 0.34f},
            {RewardType.EliteCard, 0.33f},
            {RewardType.MaxHealth, 0.33f}
        };

        public void GenerateRewards()
        {
            var isEliteRound = RunProgress.Current.BattleNumber != 0 && 
                               RunProgress.Current.BattleNumber % ScenarioLoader.BattlesPerDifficulty == 0;
        
            var weightings = isEliteRound
                ? EliteWeightings
                : NormalWeightings;
            
            var generatedRewardTitles = new List<string>();

            for (var i = 0; i < RunProgress.Current.PlayerStats.RewardOffersPerBattle; i++)
            {
                // Don't repeat rewards
                while (true)
                {
                    var newReward = GenerateReward(weightings);
                    var newRewardTitle = newReward.GetTitle();

                    if (generatedRewardTitles.Any(r => r == newRewardTitle)) continue;
                    
                    var newRewardOffer = Instantiate(_rewardOfferPrefab, _rewardOfferGrid);
                    newRewardOffer.GetComponent<RewardOffer>().Init(newReward);
                    generatedRewardTitles.Add(newRewardTitle);
                    break;
                }
            }
        }
        

        private static Reward GenerateReward(Dictionary<RewardType, float> weightings)
        {

            var rand = Random.value;

            foreach (var kvp in weightings)
            {
                var weighting = kvp.Value;

                if (rand <= weighting)
                {
                    var disturbanceAsset = RewardLoader.GetReward(kvp.Key);

                    switch (disturbanceAsset.rewardType)
                    {
                        case RewardType.Item:
                        case RewardType.EliteItem:
                            return GenerateNewItemReward(disturbanceAsset);
                        case RewardType.Card:
                        case RewardType.EliteCard:
                            return GenerateNewCardReward(disturbanceAsset);
                        default:
                            return new Reward(disturbanceAsset, disturbanceAsset.modifier);
                    }
                }

                rand -= weighting;
            }

            Debug.Log("Error: no weighting found for " + rand);
            return null;
        }

        private static ItemReward GenerateNewItemReward(RewardAsset rewardAsset)
        {
            var randomItemAsset = rewardAsset.rewardType == RewardType.EliteItem
                ? ItemLoader.EliteItemAssets.GetRandom()
                : ItemLoader.ShopItemAssets.GetRandom();

            var item = new ItemInstance(randomItemAsset, randomItemAsset.modifier);
                    
            return new ItemReward(rewardAsset,rewardAsset.modifier, item);
        }

        private static CardReward GenerateNewCardReward(RewardAsset rewardAsset)
        {
            var design = rewardAsset.rewardType == RewardType.EliteCard 
                ? DesignFactory.GenerateRandomEliteDesign() : DesignFactory.GenerateStoreDesign();

            return new CardReward(rewardAsset, rewardAsset.modifier, design);
        }
    }
}
