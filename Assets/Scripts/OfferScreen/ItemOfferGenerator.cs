using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;

namespace OfferScreen
{
    public class ItemOfferGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject itemOfferPrefab;
        [SerializeField] private Transform itemGrid;
        
        public IEnumerable<ItemOfferDisplay> ItemOfferDisplays => itemGrid.GetComponentsInChildren<ItemOfferDisplay>();

        public void Initialise()
        {
            var offeredItemIds = new List<string>();

            // Load locked items
            foreach (var itemOffer in RunProgress.OfferStorage.LockedItemOffers)
            {
                CreateItemOfferDisplay(itemOffer, true);
                offeredItemIds.Add(itemOffer.ItemId);
            }

            // If we've already generated the items (haven't completed a battle since), load the unlocked items from
            // before. Otherwise, generate new ones
            if (RunProgress.HasGeneratedMapEvents)
            {
                foreach (var itemOffer in RunProgress.OfferStorage.UnlockedItemOffers)
                {
                    CreateItemOfferDisplay(itemOffer, false);
                    offeredItemIds.Add(itemOffer.ItemId);
                }
            }
            else
            {
                for (var i = RunProgress.OfferStorage.LockedItemOffers.Count;
                    i < RunProgress.PlayerProgress.NumberOfItemsToOffer;
                    i++)
                {
                    CreateItemOfferDisplay(GenerateNewItemOffer(offeredItemIds), false);
                }
            }
        }

        private void CreateItemOfferDisplay(ItemOffer itemOffer, bool isLocked)
        {
            var itemOfferGameObject = Instantiate(itemOfferPrefab, itemGrid);
            var itemOfferDisplay = itemOfferGameObject.GetComponent<ItemOfferDisplay>();
            
            itemOfferDisplay.ItemOffer = itemOffer;
            itemOfferDisplay.Sprite = ItemLoader.GetItemSprite(itemOffer.ItemId);
            itemOfferDisplay.isLocked = isLocked;
        }
        
        private static ItemOffer GenerateNewItemOffer(IReadOnlyCollection<string> otherItems)
        {
            string itemId;
            
            while (true)
            { 
                itemId = ItemLoader.GetRandomItem(); 
                if (otherItems.FirstOrDefault(id => id == itemId) == null)
                    break;
            }

            return ItemLoader.CreateItemOffer(itemId);
        }
    }
}