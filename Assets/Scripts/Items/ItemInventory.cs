using System.Collections.Generic;
using System.Linq;
using EventListeners;
using Items.Items;
using UnityEngine;

namespace Items
{
    public class ItemInventory : MonoBehaviour
    {
        private List<Item> _items = new List<Item>();

        public IEnumerable<ItemAsset> ItemAssets => _items.Select(i => i.ItemAsset);

        public IEnumerable<IStartOfBattleListener> StartOfBattleListeners => _items
            .OfType<IStartOfBattleListener>();
    
        public IEnumerable<IEnemyAttackedListener> EnemyAttackedListeners => _items
            .OfType<IEnemyAttackedListener>();
    
        public IEnumerable<IEnemyHealedListener> EnemyHealedListeners => _items
            .OfType<IEnemyHealedListener>();

        public IEnumerable<IDamageFlatModifier> DamageFlatModifiers => _items.OfType<IDamageFlatModifier>();
        public IEnumerable<IDamageMultiplierModifier> DamageMultiplierModifiers => _items.OfType<IDamageMultiplierModifier>();

        public void AddItem(ItemAsset itemAsset)
        {
            var itemType = ItemLoader.ItemTypes[itemAsset];
            var newItem = gameObject.AddComponent(itemType).GetComponent<Item>();
            newItem.SetAsset(itemAsset);
            _items.Add(newItem);
        }
    
        public void WipeInventory()
        {
            foreach (var item in _items)
            {
                Destroy(item);
            }
        
            _items = new List<Item>();
        }
    
        public void StartBattle()
        {
        
        }
    }
}
