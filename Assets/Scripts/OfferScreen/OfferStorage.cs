using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace OfferScreen
{
    public class OfferStorage
    {
        public List<Design> Deck { get; private set; }
        
        public readonly List<Design> LockedDesignOffers = new List<Design>();
        public readonly List<Design> UnlockedDesignOffers = new List<Design>();
        
        public readonly List<ItemInfo> LockedItemOffers = new List<ItemInfo>();
        public readonly List<ItemInfo> UnlockedItemOffers = new List<ItemInfo>();
        
        public void StoreOffers(IEnumerable<DesignCard> deckRow,
            IEnumerable<DesignCard> designOfferRow, IEnumerable<ItemOffer> itemOffers)
        {
            Deck = deckRow.Select(designCard => designCard.Design).ToList();
            LockedDesignOffers.Clear();
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
                var itemInfo = itemOffer.ItemInfo;
                
                if (itemOffer.isLocked)
                {
                    LockedItemOffers.Add(itemInfo);
                }
                else
                {
                    UnlockedItemOffers.Add(itemInfo);
                }
            }
            
            Debug.Log(LockedDesignOffers.Count);
            Debug.Log(UnlockedDesignOffers.Count);
        }
    }
}