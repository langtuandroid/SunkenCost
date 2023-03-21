using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OfferScreen
{
    public class ItemOfferGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject itemOfferPrefab;
        [SerializeField] private Transform itemGrid;
        
        public IEnumerable<ItemOfferDisplay> ItemOfferDisplays => itemGrid.GetComponentsInChildren<ItemOfferDisplay>();

        public void Initialise()
        {
            var offeredItemAssets = new List<ItemAsset>();
            
            // Don't show the already equipped items
            offeredItemAssets.AddRange(RunProgress.ItemInventory.ItemAssets);

            // Load locked items
            foreach (var itemOffer in RunProgress.OfferStorage.LockedItemOffers)
            {
                CreateItemOfferDisplay(itemOffer, true);
                offeredItemAssets.Add(itemOffer.itemAsset);
            }

            // If we've already generated the items (haven't completed a battle since), load the unlocked items from
            // before. Otherwise, generate new ones
            if (RunProgress.HasGeneratedMapEvents)
            {
                foreach (var itemOffer in RunProgress.OfferStorage.UnlockedItemOffers)
                {
                    CreateItemOfferDisplay(itemOffer, false);
                    offeredItemAssets.Add(itemOffer.itemAsset);
                }
            }
            else
            {
                for (var i = RunProgress.OfferStorage.LockedItemOffers.Count;
                    i < RunProgress.PlayerStats.NumberOfItemsToOffer;
                    i++)
                {
                    var nextItemAsset = ItemLoader.ShopItemAssets.GetRandomNonDuplicate(offeredItemAssets);
                    offeredItemAssets.Add(nextItemAsset);
                    var newItemOffer = new ItemOffer(nextItemAsset);
                    CreateItemOfferDisplay(newItemOffer, false);
                }
            }
        }

        private void CreateItemOfferDisplay(ItemOffer itemOffer, bool isLocked)
        {
            var itemOfferGameObject = Instantiate(itemOfferPrefab, itemGrid);
            var itemOfferDisplay = itemOfferGameObject.GetComponent<ItemOfferDisplay>();
            itemOfferDisplay.ItemOffer = itemOffer;
            itemOfferDisplay.isLocked = isLocked;
        }
    }
}