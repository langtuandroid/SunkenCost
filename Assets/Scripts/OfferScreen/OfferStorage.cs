﻿using System.Collections.Generic;
using System.Linq;
using Designs;
using Items;
using UnityEngine;

namespace OfferScreen
{
    public class OfferStorage
    {
        public readonly List<Design> LockedDesignOffers = new List<Design>();
        public readonly List<Design> RewardDesignOffers = new List<Design>();
        public readonly List<Design> UnlockedDesignOffers = new List<Design>();

        public readonly List<ItemOffer> LockedItemOffers = new List<ItemOffer>();
        public readonly List<ItemOffer> UnlockedItemOffers = new List<ItemOffer>();

        public void StoreOffers(IEnumerable<DesignCard> deckRow,
            IEnumerable<DesignCard> designOfferRow, IEnumerable<ItemOfferDisplay> itemOffers)
        {
            RunProgress.Current.PlayerStats.SaveDeck(deckRow.Select(designCard => designCard.Design).ToList());
            LockedDesignOffers.Clear();
            RewardDesignOffers.Clear();
            UnlockedDesignOffers.Clear();
            LockedItemOffers.Clear();
            UnlockedItemOffers.Clear();
            
            foreach (var designOffer in designOfferRow)
            {
                var design = designOffer.Design;
                
                if (designOffer.isLocked)
                {
                    LockedDesignOffers.Add(design);
                }
                else
                {
                    UnlockedDesignOffers.Add(design);
                }
            }
            
            foreach (var itemOffer in itemOffers)
            {
                var itemInfo = itemOffer.ItemOffer;
                
                if (itemOffer.isLocked)
                {
                    LockedItemOffers.Add(itemInfo);
                }
                else
                {
                    UnlockedItemOffers.Add(itemInfo);
                }
            }
        }

        public void IncreaseCostOfLockedOffers()
        {
            foreach (var design in LockedDesignOffers)
            {
                design.SetCost(design.Cost + 1);
            }

            foreach (var itemOffer in LockedItemOffers)
            {
                itemOffer.SetCost(itemOffer.Cost + 1);
            }
        }
    }
}