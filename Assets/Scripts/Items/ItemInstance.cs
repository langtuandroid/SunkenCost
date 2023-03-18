using UnityEngine;

namespace Items
{
    public class ItemInstance
    {
        public readonly ItemAsset itemAsset;
        public int modifier;
        
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Sprite Sprite { get; private set; }
        public ItemRarity Rarity { get; private set; }

        public ItemInstance(ItemAsset itemAsset, int modifier)
        {
            this.itemAsset = itemAsset;
            this.modifier = modifier;
            
            Title = this.itemAsset.title;
            Description = this.itemAsset.GetDescription(this.modifier);
            Sprite = this.itemAsset.sprite;
            Rarity = this.itemAsset.rarity;
        }

        public void Modify(int amount)
        {
            modifier += amount;
        }
    }
}