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
            var offeredItemAssets = new List<ItemAsset>();

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
                    var nextItemAsset = GenerateNextItemAsset(offeredItemAssets);
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
        
        private static ItemAsset GenerateNextItemAsset(IReadOnlyCollection<ItemAsset> otherItemAssets)
        {
            ItemAsset itemAsset;
            
            while (true)
            { 
                itemAsset = GetRandomItemAsset(); 
                if (otherItemAssets.FirstOrDefault(i => i == itemAsset) == null)
                    break;
            }

            return itemAsset;
        }
        
        private static ItemAsset GetRandomItemAsset()
        {
            return ItemLoader.ItemTypes.ElementAt(Random.Range(0, ItemLoader.ItemTypes.Count)).Key;
        }
    }
}