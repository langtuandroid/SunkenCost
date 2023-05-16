using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "TestingConfig", menuName = "TestingConfig", order = 0)]
    public class TestingConfig : ScriptableObject
    {
        [field: SerializeField] public bool IsActive { get; private set; }
        [field: SerializeField] public List<string> StartingDeck { get; private set; } = new List<string>();
        [field: SerializeField] public List<string> StartingItems { get; private set; } = new List<string>();
        
        [field: SerializeField] public List<EnemyType> StartingEnemies { get; private set; } = new List<EnemyType>();

        public List<Type> GetStartingItemTypes()
        {
            var typeList = new List<Type>();

            foreach (var item in StartingItems)
            {
                var type = Type.GetType(item);
                if (type is null)
                {
                    throw new Exception("No type for " + item + " found!");
                }
                
                typeList.Add(type);
            }

            return typeList;
        }
    }
}