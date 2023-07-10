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
            //offeredItemAssets.AddRange(RunProgress.ItemInventory.ItemAssets);

            // Load locked items
            foreach (var itemOffer in RunProgress.Current.OfferStorage.LockedItemOffers)
            {
                CreateItemOfferDisplay(itemOffer, true);
                offeredItemAssets.Add(itemOffer.itemAsset);
            }

            // If we've already generated the items (haven't completed a battle since), load the unlocked items from
            // before. Otherwise, generate new ones
            if (RunProgress.Current.HasGeneratedMapEvents)
            {
                foreach (var itemOffer in RunProgress.Current.OfferStorage.UnlockedItemOffers)
                {
                    CreateItemOfferDisplay(itemOffer, false);
                    offeredItemAssets.Add(itemOffer.itemAsset);
                }
            }
            else
            {
                var amountOfUnlockedOffers = RunProgress.Current.OfferStorage.LockedItemOffers.Count;
                var amountToOffer = RunProgress.Current.PlayerStats.ItemOffersPerBattle - amountOfUnlockedOffers;
                GenerateUnlockedItemOffers(amountToOffer, offeredItemAssets);
            }
        }

        public void ReRoll()
        {
            var offeredAssets = new List<ItemAsset>();
            foreach (var itemOfferDisplay in ItemOfferDisplays)
            {
                if (itemOfferDisplay.isLocked)
                {
                    offeredAssets.Add(itemOfferDisplay.ItemOffer.itemAsset);
                    continue;
                }
                
                Destroy(itemOfferDisplay.gameObject);
            }

            var amountToOffer = RunProgress.Current.PlayerStats.ItemOffersPerBattle - offeredAssets.Count();
            GenerateUnlockedItemOffers(amountToOffer, offeredAssets);
        }

        private void GenerateUnlockedItemOffers(int amountOfNewOffers, ICollection<ItemAsset> offeredItemAssets)
        {
            for (var i = 0; i < amountOfNewOffers; i++)
            {
                var nextItemAsset = ItemLoader.ShopItemAssets.GetRandomNonDuplicate(offeredItemAssets);
                offeredItemAssets.Add(nextItemAsset);
                var newItemOffer = new ItemOffer(nextItemAsset);
                CreateItemOfferDisplay(newItemOffer, false);
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