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
        
        public void CreateDesignCards()
        { 
            CreateBatchOfDesignCards(RunProgress.Current.PlayerStats.Deck, deckRow, false);
            CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.RewardDesignOffers, offerRow);
            CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.LockedDesignOffers, offerRow, locked: true);
            
            if (RunProgress.Current.HasGeneratedMapEvents) 
                CreateBatchOfDesignCards(RunProgress.Current.OfferStorage.UnlockedDesignOffers, offerRow);
            else 
                GenerateNewUnlockedCards(RunProgress.Current.PlayerStats.DesignOffersPerBattle - 
                                           RunProgress.Current.OfferStorage.LockedDesignOffers.Count);
        }

        public void ReRoll()
        {
            DiscardLockedCards();

            var amountOfLockedCards = FindObjectsOfType<DesignCard>().Count(d => d.isLocked);
            var amountToCreate = RunProgress.Current.PlayerStats.DesignOffersPerBattle - amountOfLockedCards;
            GenerateNewUnlockedCards(amountToCreate);
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

        private void CreateBatchOfDesignCards(IEnumerable<Design> designs, Transform row,
            bool lockable = true, bool locked = false)
        {
            foreach (var design in designs)
            {
                CreateDesignCardFromDesign(design, row, locked, lockable);
            }
        }
        
        private void GenerateNewUnlockedCards(int amountToCreate)
        {
            for (var i = 0; i < amountToCreate; i++)
            {
                var design = DesignFactory.GenerateStoreDesign();
                CreateDesignCardFromDesign(design, offerRow);
            }
        }

        private void CreateDesignCardFromDesign(Design design, Transform row, bool locked = false, bool lockable = true)
        {
            var newCardObject = Instantiate(designCardPrefab, row);
            var info = newCardObject.GetComponentInChildren<DesignDisplay>();
            info.design = design;

            var designCard = newCardObject.GetComponent<DesignCard>();
            designCard.isLocked = locked;
        }
    }
}