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

    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private ItemShaderDisplay _itemShaderDisplay;
        
        [SerializeField] private TooltipTrigger _tooltipTrigger;

        private ItemInstance _itemInstance;

        private void Start()
        {
            UpdateDisplay();
        }

        public void SetItemInstance(ItemInstance itemInstance)
        {
            _itemInstance = itemInstance;
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
        
        public void UpdateDisplay()
        {
            SetTitle(_itemInstance.Title);
            SetDescription(_itemInstance.Description);
            SetSprite(_itemInstance.Sprite);
        }
        
        public void Activate()
        {
            _itemShaderDisplay.Activate();
        }

        private void SetTitle(string title)
        {
            _tooltipTrigger.header = title;
        }

        private void SetDescription(string desc)
        {
            _tooltipTrigger.content = desc;
        }

        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}