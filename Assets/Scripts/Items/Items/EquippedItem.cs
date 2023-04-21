using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
using UI;
using UnityEngine;

namespace Items.Items
{
    public abstract class EquippedItem : BattleEventResponder, IBattleEventUpdatedUI
    {
        public ItemInstance ItemInstance { get; private set; }
        protected int Amount => ItemInstance.modifier;
        
        private Queue<ItemDisplayState> _savedStates = new Queue<ItemDisplayState>();

        private void Start()
        {
            FindObjectOfType<BattleEventsManager>().RegisterUIUpdater(this);
        }

        public void SetInstance(ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;
        }
        
        public void StartVisualisationCoroutine(BattleEvent battleEvent)
        {
            // Glow the icon
            ItemIconsDisplay.Current.ActivateItemDisplay(ItemInstance);
        }
        
        public bool GetIfUpdating(BattleEvent battleEvent)
        {
            return battleEvent.item.ItemInstance == ItemInstance;
        }

        public void SaveCurrentState()
        {
            var state = new ItemDisplayState(ItemInstance.Title, ItemInstance.Description);
            _savedStates.Enqueue(state);
        }

        public void LoadNextState()
        {
            ItemIconsDisplay.Current.ActivateItemDisplay(ItemInstance);
            ItemIconsDisplay.Current.RefreshItem(ItemInstance, _savedStates.Dequeue());
        }
    }
}