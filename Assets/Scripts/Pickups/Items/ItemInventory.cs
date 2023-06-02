using System;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using Items.Items;
using UnityEngine;

namespace Items
{
    public class ItemInventory : MonoBehaviour
    {
        private List<EquippedItem> _items = new List<EquippedItem>();

        public IEnumerable<BattleEventResponder> AllItemsAsBattleEventResponders =>
            _items.Select(i => i as BattleEventResponder);

        public IEnumerable<ItemInstance> ItemInstances => _items.Select(i => i.ItemInstance).ToArray();
        public IEnumerable<ItemAsset> ItemAssets => _items.Select(i => i.ItemInstance.itemAsset);

        public void AddItem(ItemInstance itemInstance)
        {
            var itemType = ItemLoader.ItemAssetToTypeDict[itemInstance.itemAsset];
            
            if (gameObject.AddComponent(itemType) is EquippedItem newItem)
            {
                newItem.SetInstance(itemInstance);
                _items.Add(newItem);

                if (newItem is IHasPickupAction pickupActionItem)
                {
                    pickupActionItem.OnPickup();
                }
            }
            else
            {
                throw new Exception("Equipped item is not an equipped item?");
            }
        }
    
        public void WipeInventory()
        {
            foreach (var item in _items)
            {
                Destroy(item);
            }
        
            _items = new List<EquippedItem>();
        }
    }
}
