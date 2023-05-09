using System.Collections.Generic;
using Disturbances;
using Items;
using OfferScreen;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MapScreen
{
    public class DisturbanceGenerator : MonoBehaviour
    {
        [SerializeField] private DisturbanceEvent topDisturbanceEvent;
        [SerializeField] private DisturbanceEvent bottomDisturbanceEvent;
    
    
        private static readonly Dictionary<DisturbanceType, float> NormalWeightings = new Dictionary<DisturbanceType, float>()
        {
            {DisturbanceType.GoldRush, 0.35f},
            {DisturbanceType.Heart, 0.25f},
            {DisturbanceType.Card, 0.2f},
            {DisturbanceType.Item, 0.2f}
        };
    
        private static readonly Dictionary<DisturbanceType, float> EliteWeightings = new Dictionary<DisturbanceType, float>()
        {
            {DisturbanceType.EliteItem, 0.34f},
            {DisturbanceType.EliteCard, 0.33f},
            {DisturbanceType.MaxHealth, 0.33f}
        };

        private void Awake()
        {
            if (RunProgress.HasGeneratedMapEvents)
            {
                topDisturbanceEvent.Init(RunProgress.GeneratedMapDisturbances[0]);
                bottomDisturbanceEvent.Init(RunProgress.GeneratedMapDisturbances[1]);
                
                return;
            }
            
            var isEliteRound = RunProgress.BattleNumber % ScenarioLoader.BATTLES_PER_DIFFICULTY 
                             == ScenarioLoader.BATTLES_PER_DIFFICULTY - 1;
        
            var weightings = isEliteRound
                ? EliteWeightings
                : NormalWeightings;
            
            var generatedDisturbances = new List<Disturbance>();
        
            var topDisturbance = GenerateDisturbance(weightings);
            topDisturbanceEvent.Init(topDisturbance);
            generatedDisturbances.Add(topDisturbance);

            for (var i = 0; i < 1000; i++)
            {
                if (i == 999)
                {
                    Debug.Log("Couldn't generate a disturbance!");
                }
            
                // Duplicates of items or cards are ok, but not anything else
                var bottomDisturbance = GenerateDisturbance(weightings);
                if (bottomDisturbance.DisturbanceType != DisturbanceType.Card 
                    && bottomDisturbance.DisturbanceType != DisturbanceType.Item
                    && bottomDisturbance.DisturbanceType == topDisturbance.DisturbanceType) continue;
            
                bottomDisturbanceEvent.Init(bottomDisturbance);
                generatedDisturbances.Add(bottomDisturbance);
                break;
            }
            
            RunProgress.HaveGeneratedDisturbanceEvents(generatedDisturbances);
        }
        

        private static Disturbance GenerateDisturbance(Dictionary<DisturbanceType, float> weightings)
        {

            var rand = Random.value;

            foreach (var kvp in weightings)
            {
                var weighting = kvp.Value;

                if (rand <= weighting)
                {
                    var disturbanceAsset = DisturbanceLoader.GetDisturbance(kvp.Key);

                    switch (disturbanceAsset.disturbanceType)
                    {
                        case DisturbanceType.Item:
                        case DisturbanceType.EliteItem:
                            return GenerateNewItemDisturbance(disturbanceAsset);
                        case DisturbanceType.Card:
                        case DisturbanceType.EliteCard:
                            return GenerateNewCardDisturbance(disturbanceAsset);
                        default:
                            return new Disturbance(disturbanceAsset, disturbanceAsset.amount);
                    }
                }

                rand -= weighting;
            }

            Debug.Log("Error: no weighting found for " + rand);
            return null;
        }

        private static ItemDisturbance GenerateNewItemDisturbance(DisturbanceAsset disturbanceAsset)
        {
            var randomItemAsset = disturbanceAsset.disturbanceType == DisturbanceType.EliteItem
                ? ItemLoader.EliteItemAssets.GetRandom()
                : ItemLoader.ShopItemAssets.GetRandom();

            var item = new ItemInstance(randomItemAsset, randomItemAsset.modifier);
                    
            return new ItemDisturbance(disturbanceAsset,disturbanceAsset.amount, item);
        }

        private static CardDisturbance GenerateNewCardDisturbance(DisturbanceAsset disturbanceAsset)
        {
            var design = disturbanceAsset.disturbanceType == DisturbanceType.EliteCard 
                ? DesignFactory.GenerateRandomRareDesign() : DesignFactory.GenerateStoreDesign();

            return new CardDisturbance(disturbanceAsset, disturbanceAsset.amount, design);
        }
    }
}
