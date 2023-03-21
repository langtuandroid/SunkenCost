using System.Collections;
using System.Collections.Generic;
using MapScreen;
using UnityEngine;
using UnityEngine.Serialization;

namespace Disturbances
{
    [CreateAssetMenu(menuName = "Disturbance")]
    public class DisturbanceAsset : ScriptableObject
    {
        public DisturbanceType disturbanceType;
        public Sprite sprite;
        public string title;
        public string description;
        public int amount;

        public void ModifyAmount(int modifyAmount)
        {
            amount += modifyAmount;
        }
    }
}