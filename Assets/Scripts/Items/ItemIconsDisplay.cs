using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Items;
using UnityEngine;

namespace UI
{
    public class ItemIconsDisplay : MonoBehaviour
    {
        public static ItemIconsDisplay Current;
        
        private Dictionary<ItemInstance, ItemDisplay> _itemDisplaysAndInstances =
            new Dictionary<ItemInstance, ItemDisplay>();
        
        [SerializeField] private Transform displayGrid;
        [SerializeField] private GameObject itemIconDisplayPrefab;

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
        }

        private void Start()
        {
            foreach (var itemInstance in RunProgress.ItemInventory.ItemInstances)
            {
                AddItemToDisplay(itemInstance);
            }
        }

        public void AddItemToDisplay(ItemInstance itemInstance)
        {
            var newItemIconGameObject = Instantiate(itemIconDisplayPrefab, displayGrid);
            var newItemDisplay = newItemIconGameObject.GetComponent<ItemDisplay>();
            newItemDisplay.SetItemInstance(itemInstance);
            
            _itemDisplaysAndInstances.Add(itemInstance, newItemDisplay);
        }

        public void ActivateItemDisplay(ItemInstance itemInstance)
        {
            if (_itemDisplaysAndInstances.TryGetValue(itemInstance, out var itemDisplay))
            {
                itemDisplay.Activate();
            }
            else
            {
                throw new Exception("Couldn't activate " + itemInstance.Title + "'s display");
            }
        }
    }
}