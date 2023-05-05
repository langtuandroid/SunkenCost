using System;
using System.Collections;
using BattleScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BoatHealthMeter : MonoBehaviour
    {
        [SerializeField] private GameObject _healthMeter;
        [SerializeField] private TextMeshProUGUI _healthText;

        private const float TWEEN_TIME = 0.3f;
        private const int FULL_HEALTH_Y_POSITION = 0;
        private const int ZERO_HEALTH_Y_POSITION = 375;
        private const int POSITION_DIFFERENCE = FULL_HEALTH_Y_POSITION - ZERO_HEALTH_Y_POSITION;

        private Color _defaultColor;
        private RectTransform _healthMeterRectTransform;
        private Image _healthMeterImage;

        private int _lastHealth;
        private int _lastMaxHealth;

        private void Awake()
        {
            _healthMeterRectTransform = _healthMeter.GetComponent<RectTransform>();
            _healthMeterImage = _healthMeter.GetComponent<Image>();
            
            _defaultColor = _healthText.color;
            _lastHealth = RunProgress.PlayerStats.Health;
            _lastMaxHealth = RunProgress.PlayerStats.MaxHealth;

            UpdateHealthText(_lastHealth, _lastMaxHealth);

            var yOffset = GetYOffset(_lastHealth, _lastMaxHealth);
            _healthMeterRectTransform.localPosition = new Vector3(_healthMeterRectTransform.localPosition.x, yOffset, 1);
        }

        public void RefreshMeter(int health, int maxHealth)
        {
            LeanTween.cancel(gameObject);

            var yOffset = GetYOffset(health, maxHealth);

            var healthMeterLocalPosition = _healthMeterRectTransform.localPosition;
            LeanTween.moveLocal(_healthMeter,new Vector3(healthMeterLocalPosition.x, yOffset, 1), TWEEN_TIME)
                .setEaseSpring();

            UpdateHealthText(health, maxHealth);

            if (health < _lastHealth || maxHealth < _lastMaxHealth)
            {
                Glow(Color.red);
            }
            else if (health > _lastHealth || maxHealth > _lastHealth)
            {
                Glow(Color.green);
            }
            
            _lastHealth = health;
            _lastMaxHealth = maxHealth;
        }

        private float GetYOffset(int health, int maxHealth)
        {
            var percentageOfHealthLost = health > 0 ? 1 - (float)health / maxHealth : 1f;
            return POSITION_DIFFERENCE * percentageOfHealthLost;
        }

        private void UpdateHealthText(int health, int maxHealth)
        {
            _healthText.text = health + "/" + maxHealth;
        }

        private void Glow(Color color)
        {
            StopAllCoroutines();
            StartCoroutine(GlowColor(color));
        }

        private IEnumerator GlowColor(Color color)
        {
            _healthText.color = color;
            _healthMeterImage.color = color;
            yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed);
            _healthText.color = _defaultColor;
            _healthMeterImage.color = Color.white;
        }
    }
}