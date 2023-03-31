using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OfferScreen
{
    public static class DesignFactory
    {
        private static int Progress => RunProgress.BattleNumber;
        
        private static Design InstantiateDesign(string designName)
        {
            var designType = DesignManager.GetDesignType(designName);
            return (Design)Activator.CreateInstance(designType);
        }
        
        public static Design GenerateStoreDesign()
        {
            const double maxProgress = 15.0;
            
            var chancesOnFirstBattle = new[] {0.77, 0.26, 0.02};
            var chancesOnLastPossibleBattle = new[] {0.2, 0.6, 0.2};

            var percentage = Progress / maxProgress;
            Debug.Log(percentage);

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
            return InstantiateDesign(DesignManager.CommonDesigns.GetRandomElement());
        }
        
        public static Design GenerateRandomUncommonDesign()
        {
            return InstantiateDesign(DesignManager.UncommonDesigns.GetRandomElement());
        }
        
        public static Design GenerateRandomRareDesign()
        {
            return InstantiateDesign(DesignManager.RareDesigns.GetRandomElement());
        }
    }
}