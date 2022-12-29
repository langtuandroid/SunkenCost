using System;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items")]
    public class Item : ScriptableObject
    {
        public string title;
        public string description;

        public Sprite sprite;
        public Component inGameItemScript;
    }
}
