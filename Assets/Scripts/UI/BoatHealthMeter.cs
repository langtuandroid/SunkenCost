using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BoatHealthMeter : MonoBehaviour
    {
        [SerializeField] private RectTransform _healthMeterRectTransform;
        [SerializeField] private TextMeshProUGUI _healthText;
        
        private const int FULL_HEALTH_Y_POSITION = 0;
        private const int ZERO_HEALTH_Y_POSITION = 375;
        private const int POSITION_DIFFERENCE = FULL_HEALTH_Y_POSITION - ZERO_HEALTH_Y_POSITION;

        private int _lastHealth;
        private int _lastMaxHealth;

        private void Awake()
        {
            RefreshMeter(RunProgress.PlayerStats.Health, RunProgress.PlayerStats.MaxHealth);
        }

        public void RefreshMeter(int health, int maxHealth)
        {
            var percentageOfHealthLost = health > 0 ? 1 - (float)health / maxHealth : 1f;
            var yOffset = POSITION_DIFFERENCE * percentageOfHealthLost;
            
            var healthMeterLocalPosition = _healthMeterRectTransform.localPosition;
            _healthMeterRectTransform.localPosition = new Vector3(healthMeterLocalPosition.x, yOffset, 1);

            _healthText.text = health + "/" + maxHealth;
        }
    }
}