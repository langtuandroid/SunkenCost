using System;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public readonly struct ItemDisplayState
    {
        public readonly string title;
        public readonly string description;

        public ItemDisplayState(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
    }
    
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image image;
        
        [SerializeField] private TooltipTrigger tooltipTrigger;

        private ItemInstance _itemInstance;

        private void Start()
        {
            UpdateDisplay(GetCurrentState());
        }

        public void SetItemInstance(ItemInstance itemInstance)
        {
            _itemInstance = itemInstance;
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
        
        public void UpdateDisplay(ItemDisplayState itemDisplayState)
        {
            SetTitle(itemDisplayState.title);
            SetDescription(itemDisplayState.description);
        }

        private void SetTitle(string title)
        {
            tooltipTrigger.header = title;
        }

        private void SetDescription(string desc)
        {
            tooltipTrigger.content = desc;
        }

        private void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        private ItemDisplayState GetCurrentState()
        {
            return new ItemDisplayState(_itemInstance.Title, _itemInstance.Description);
        }
    }
}