using UnityEngine;

namespace Items.Items
{
    public class EquippedItem : MonoBehaviour
    {
        public ItemInstance ItemInstance { get; private set; }

        public string Description => ItemInstance.Description;
        public int Amount => ItemInstance.modifier;

        public void SetInstance(ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;
        }
    }
}