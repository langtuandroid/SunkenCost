using System.Collections;
using BattleScreen;
using UnityEngine;

namespace Items.Items
{
    public abstract class EquippedItem : MonoBehaviour
    {
        public ItemInstance ItemInstance { get; private set; }
        protected int Amount => ItemInstance.modifier;

        public void SetInstance(ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;
        }
    }
}