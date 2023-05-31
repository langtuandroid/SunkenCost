using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Designs;
using Designs.UI;
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
            allCards.AddRange(CreateRewardCards());
            
            allCards.AddRange(RunProgress.Current.HasGeneratedMapEvents
                ? CreateSavedUnlockedCards()
                : GenerateNewUnlockedCards(RunProgress.Current.PlayerStats.NumberOfCardsToOffer - 
                                           RunProgress.Current.OfferStorage.LockedDesignOffers.Count));

            return allCards;
        }

        public List<DesignCard> ReRoll()
        {
            DiscardLockedCards();

            var amountOfLockedCards = FindObjectsOfType<DesignCard>().Count(d => d.isLocked);
            var amountToCreate = RunProgress.Current.PlayerStats.NumberOfCardsToOffer - amountOfLockedCards;
            return GenerateNewUnlockedCards(amountToCreate);
        }
        
        private void DiscardLockedCards()
        {
            var cards = CardsInLeaveRow.ToArray();
            foreach (var card in cards)
            {
                if (card.isLocked) continue;
                Destroy(card.gameObject);
            }
        }

        private IEnumerable<DesignCard> CreateBatchOfDesignCards(IEnumerable<Design> designs, Transform row,
            bool lockable = true, bool locked = false)
        {
            return designs.Select(design => CreateDesignCardFromDesign(design, row, locked, lockable));
        }

        private IEnumerable<DesignCard> CreateDeckCards()
        {
            return CreateBatchOfDesignCards(RunProgress.Current.PlayerStats.Deck, deckRow, false);
        }

        private IEnumerable<DesignCard> CreateRewardCards()
        {
            return CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.RewardDesignOffers, offerRow);
        }

        private IEnumerable<DesignCard> CreateLockedCards()
        {
            return CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.LockedDesignOffers, offerRow, locked: true);
        }

        private IEnumerable<DesignCard> CreateSavedUnlockedCards()
        {
            return CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.UnlockedDesignOffers, offerRow);
        }

        private List<DesignCard> GenerateNewUnlockedCards(int amountToCreate)
        {
            var newCards = new List<DesignCard>();
            for (var i = 0; i < amountToCreate; i++)
            {
                var design = DesignFactory.GenerateStoreDesign();
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

        
    }
}