using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace OfferScreen
{
    public class DesignCardOfferGenerator : MonoBehaviour
    {
        [SerializeField] private Transform deckRow;
        [SerializeField] private Transform offerRow;
        [SerializeField] private GameObject designCardPrefab;
        
        public int CardsInDeckRowCount => deckRow.childCount;
        public IEnumerable<DesignCard> CardsInDeckRow => deckRow.GetComponentsInChildren<DesignCard>();
        public IEnumerable<DesignCard> CardsInLeaveRow => offerRow.GetComponentsInChildren<DesignCard>();
        
        public List<DesignCard> CreateDesignCards()
        {
            var allCards = new List<DesignCard>();
            
            allCards.AddRange(CreateDeckCards());
            allCards.AddRange(CreateLockedCards());

            allCards.AddRange(RunProgress.HasGeneratedMapEvents
                ? CreateSavedUnlockedCards()
                : GenerateNewUnlockedCards());

            return allCards;
        }

        private IEnumerable<DesignCard> CreateBatchOfDesignCards(IEnumerable<Design> designs, Transform row,
            bool lockable = true, bool locked = false)
        {
            return designs.Select(design => CreateDesignCardFromDesign(design, row, locked, lockable));
        }

        private IEnumerable<DesignCard> CreateDeckCards()
        {
            return CreateBatchOfDesignCards(RunProgress.PlayerStats.Deck, deckRow, lockable: false);
        }

        private IEnumerable<DesignCard> CreateLockedCards()
        {
            return CreateBatchOfDesignCards(RunProgress.OfferStorage.LockedDesignOffers, offerRow, locked: true);
        }

        private IEnumerable<DesignCard> CreateSavedUnlockedCards()
        {
            return CreateBatchOfDesignCards(RunProgress.OfferStorage.UnlockedDesignOffers, offerRow);
        }

        private List<DesignCard> GenerateNewUnlockedCards()
        {
            var newCards = new List<DesignCard>();
            
            var amountOfLockedCards = RunProgress.OfferStorage.LockedDesignOffers.Count;
            for (var i = amountOfLockedCards; i < RunProgress.PlayerStats.NumberOfCardsToOffer; i++)
            {
                var designName = GetDesign();
                var designType = DesignManager.GetDesignType(designName);
                var design = (Design)Activator.CreateInstance(designType);

                newCards.Add(CreateDesignCardFromDesign(design, offerRow));
            }

            return newCards;
        }

        private DesignCard CreateDesignCardFromDesign(Design design, Transform row, bool locked = false, bool lockable = true)
        {
            var newCardObject = Instantiate(designCardPrefab, row);
            var info = newCardObject.GetComponentInChildren<DesignDisplay>();
            info.design = design;

            var designCard = newCardObject.GetComponent<DesignCard>();
            designCard.isLocked = locked;

            if (lockable)
            {
                designCard.AllowLocking();
            }
            else
            {
                designCard.PreventLocking();
            }

            return designCard;
        }

        private string GetDesign()
        {
            var currentProgress = RunProgress.BattleNumber;
            var maxProgress = 50.0;
            
            var chancesOnFirstBattle = new[] {0.8, 0.5, 0.05};
            var chancesOnLastPossibleBattle = new[] {0.2, 0.6, 0.2};

            var percentage = currentProgress / maxProgress;

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
                return DesignManager.CommonDesigns[Random.Range(0, DesignManager.CommonDesigns.Count)];
            }

            randomNum -= currentChances[0];
                
            // Uncommon
            if (randomNum <= currentChances[1])
            {
                return DesignManager.UncommonDesigns[Random.Range(0, DesignManager.UncommonDesigns.Count)];
            }

            // Rare
            return DesignManager.RareDesigns[Random.Range(0, DesignManager.RareDesigns.Count)];
        }
    }
}