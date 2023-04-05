using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.ActionsAndResponses;
using BattleScreen.Events;
using Items;
using UnityEngine;

namespace UI
{
    public class ItemIconsDisplay : MonoBehaviour
    {
        private static ItemIconsDisplay _current;
        
        private Dictionary<ItemInstance, ItemDisplay> _itemDisplaysAndInstances =
            new Dictionary<ItemInstance, ItemDisplay>();
        
        [SerializeField] private Transform displayGrid;
        [SerializeField] private GameObject itemIconDisplayPrefab;

        private void Awake()
        {
            if (_current)
            {
                Destroy(_current.gameObject);
            }

            _current = this;
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

        public static void ActivateItemDisplay(ItemInstance itemInstance)
        {
            if (_current._itemDisplaysAndInstances.TryGetValue(itemInstance, out var itemDisplay))
            {
                _current.StartCoroutine(SetTempColor(itemDisplay));
            }
            else
            {
                throw new Exception("Couldn't activate " + itemInstance.Title + "'s display");
            }
        }

        private static IEnumerator SetTempColor(ItemDisplay itemDisplay)
        {
            itemDisplay.SetColor(Color.green);
            yield return new WaitForSeconds(BattleEvents.ActionExecutionSpeed);
            itemDisplay.SetColor(Color.white);
        }
    }
}