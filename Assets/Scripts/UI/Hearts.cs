using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Hearts : MonoBehaviour
    {
        [SerializeField] private GameObject heartPrefab;
        private List<Heart> _hearts = new List<Heart>();

        private void Awake()
        {
            for (var i = 0; i < RunProgress.PlayerStats.MaxHealth; i++)
            {
                CreateHeart();
            }
            
            UpdateLives(RunProgress.PlayerStats.Health);
        }

        public void UpdateLives(int lives)
        {
            for (var i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].SetHeart(lives > i);
            }
        }

        private void CreateHeart()
        {
            var heartObject = Instantiate(heartPrefab, transform);
            _hearts.Add(heartObject.GetComponent<Heart>());
        }
    }
}