using UnityEngine;
using UnityEngine.Serialization;

namespace Items
{
    public enum Rarity
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        ElitePickup = 4
    }
    
    [CreateAssetMenu(menuName = "Item")]
    public class ItemAsset : ScriptableObject
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