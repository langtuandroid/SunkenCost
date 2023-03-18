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

        public IEnumerable<ItemAsset> ItemAssets => _items.Select(i => i.ItemInstance.itemAsset);

        public IEnumerable<IStartOfBattleListener> StartOfBattleListeners => _items
            .OfType<IStartOfBattleListener>();
    
        public IEnumerable<IEnemyAttackedListener> EnemyAttackedListeners => _items
            .OfType<IEnemyAttackedListener>();
    
        public IEnumerable<IEnemyHealedListener> EnemyHealedListeners => _items
            .OfType<IEnemyHealedListener>();

        public IEnumerable<IDamageFlatModifier> DamageFlatModifiers => _items.OfType<IDamageFlatModifier>();
        public IEnumerable<IDamageMultiplierModifier> DamageMultiplierModifiers => _items.OfType<IDamageMultiplierModifier>();

        public void AddItem(ItemInstance itemInstance)
        {
            var itemType = ItemLoader.ItemTypes[itemInstance.itemAsset];
            var newItem = gameObject.AddComponent(itemType).GetComponent<EquippedItem>();
            newItem.SetInstance(itemInstance);
            _items.Add(newItem);
        }
    
        public void WipeInventory()
        {
            foreach (var item in _items)
            {
                Destroy(item);
            }
        
            _items = new List<EquippedItem>();
        }
    
        public void StartBattle()
        {
        
        }
    }
}
