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

        public void SetTitle(string title)
        {
            tooltipTrigger.header = title;
        }

        public void SetDescription(string desc)
        {
            tooltipTrigger.content = desc;
        }
        
        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundImage.color = color;
        }
    }
}