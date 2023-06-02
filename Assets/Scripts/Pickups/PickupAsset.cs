using UnityEngine;

namespace Pickups
{
    public abstract class PickupAsset : ScriptableObject
    {
        public string title;
        [SerializeField] private string description;
        public Sprite sprite;
        public int modifier;
        public Rarity rarity;

        public string GetDescription(int amount)
        {
            var desc = description;
            return desc.Replace("@", amount.ToString());
        }
    }
}