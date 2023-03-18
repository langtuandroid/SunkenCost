using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        ElitePickup
    }
    
    [CreateAssetMenu(menuName = "Item")]
    public class ItemAsset : ScriptableObject
    {
        public string title;
        [SerializeField] private string description;
        public Sprite sprite;
        public int modifier;
        public ItemRarity rarity;

        public string GetDescription(int amount)
        {
            return description.Replace("@", amount.ToString());
        }
    }
}