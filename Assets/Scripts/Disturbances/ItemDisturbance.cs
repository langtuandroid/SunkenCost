using Items;
using UnityEngine;

namespace Disturbances
{
    public class ItemDisturbance : Disturbance
    {
        public ItemInstance ItemInstance { get; private set; }

        public ItemDisturbance(DisturbanceAsset disturbanceAsset, int modifier, ItemInstance itemInstance)
            : base(disturbanceAsset, modifier)
        {
            ItemInstance = itemInstance;
        }

        public override string GetAdditionalTitle()
        {
            return base.GetDescription() + ItemInstance.Title;
        }

        public override string GetDescription()
        {
            return ItemInstance.Description;
        }
        
        public override Sprite GetSprite()
        {
            return ItemInstance.Sprite;
        }
    }
}