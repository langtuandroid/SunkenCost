using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image image;
        
        [SerializeField] private TooltipTrigger tooltipTrigger;

        private ItemInstance _itemInstance;

        private void Start()
        {
            SetTitle(_itemInstance.Title);
            SetDescription(_itemInstance.Description);
            SetSprite(_itemInstance.Sprite);
        }

        public void SetItemInstance(ItemInstance itemInstance)
        {
            _itemInstance = itemInstance;
        }
        
        public void SetBackgroundColor(Color color)
        {
            backgroundImage.color = color;
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
    }
}