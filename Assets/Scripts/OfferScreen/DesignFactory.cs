using System;
using System.Collections.Generic;
using Designs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OfferScreen
{
    public static class DesignFactory
    {
        private static int Progress => RunProgress.BattleNumber;

        public static Design InstantiateDesignFromString(string designAssetName)
        {
            var designAsset = DesignLoader.AllDesignAssetsByName[designAssetName];
            if (designAsset is null)
            {
                throw new Exception("No design asset by the name " + designAssetName + " found!");
            }
            return InstantiateDesign(designAsset);
        }

        public static Design GenerateStoreDesign()
        {
            const double maxProgress = 15.0;
            
            var chancesOnFirstBattle = new[] {0.77, 0.26, 0.02};
            var chancesOnLastPossibleBattle = new[] {0.2, 0.6, 0.2};

            var percentage = Progress / maxProgress;

            var currentChances = new double[chancesOnFirstBattle.Length];

            for (var i = 0; i < currentChances.Length; i++)
            {
                currentChances[i] = chancesOnFirstBattle[i] +
                                    (chancesOnLastPossibleBattle[i] - chancesOnFirstBattle[i]) * percentage;
                
            }

            var randomNum = (double)Random.value;
            
            // Common
            if (randomNum <= currentChances[0])
            {
                return GenerateRandomCommonDesign();
            }

            randomNum -= currentChances[0];
                
            // Uncommon
            if (randomNum <= currentChances[1])
            {
                return GenerateRandomUncommonDesign();
            }

            // Rare
            return GenerateRandomRareDesign();
        }

        public static Design GenerateRandomCommonDesign()
        {
            return InstantiateDesign(DesignLoader.CommonDesigns.GetRandomElement());
        }
        
        public static Design GenerateRandomUncommonDesign()
        {
            return InstantiateDesign(DesignLoader.UncommonDesigns.GetRandomElement());
        }
        
        public static Design GenerateRandomRareDesign()
        {
            return InstantiateDesign(DesignLoader.RareDesigns.GetRandomElement());
        }
        
        private static Design InstantiateDesign(DesignAsset designAsset)
        {
            return new Design(designAsset);
        }
    }
}