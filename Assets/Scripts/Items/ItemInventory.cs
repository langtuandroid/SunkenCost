using System;
using System.Collections.Generic;
using System.Linq;
using EventListeners;
using Items.Items;
using UnityEngine;

namespace Items
{
    public class ItemInventory : MonoBehaviour
    {
        private List<EquippedItem> _items = new List<EquippedItem>();

        public IEnumerable<ItemInstance> ItemInstances => _items.Select(i => i.ItemInstance).ToArray();
        public IEnumerable<ItemAsset> ItemAssets => _items.Select(i => i.ItemInstance.itemAsset);

        public IEnumerable<IStartOfBattleListener> StartOfBattleListeners => _items
            .OfType<IStartOfBattleListener>();
    
        public IEnumerable<IEnemyAttackedListener> EnemyAttackedListeners => _items
            .OfType<IEnemyAttackedListener>();
    
        public IEnumerable<IEnemyHealedListener> EnemyHealedListeners => _items
            .OfType<IEnemyHealedListener>();

        public IEnumerable<IEnemyReachedEndListener> EnemyReachedEndListeners => _items
            .OfType<IEnemyReachedEndListener>();

        public IEnumerable<IPlayerLostLifeListener> PlayerLostLifeListeners => _items
            .OfType<IPlayerLostLifeListener>();

        public IEnumerable<IDamageFlatModifier> DamageFlatModifiers => _items.OfType<IDamageFlatModifier>();
        public IEnumerable<IDamageMultiplierModifier> DamageMultiplierModifiers => _items.OfType<IDamageMultiplierModifier>();

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
