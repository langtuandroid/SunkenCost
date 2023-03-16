using UnityEngine;

namespace Items.Items
{
    public class Item : MonoBehaviour
    {
        public ItemAsset ItemAsset { get; private set; }

        public string Description => ItemAsset.GetDescription(Amount);
        protected int Amount { get; private set; }

        public void SetAsset(ItemAsset itemAsset)
        {
            ItemAsset = itemAsset;
            Amount = itemAsset.amount;
        }
    }
}