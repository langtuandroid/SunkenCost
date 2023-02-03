using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MapScreen
{

    public enum MapEventType
    {
        None,
        Coin,
        Heart,
        UpgradeCard,
        SpecificCard
    }

    public class MapEvent : MonoBehaviour
    {
        private Button _button;

        public MapEventType EventType { get; set; } = MapEventType.None;

        [SerializeField] private TextMeshProUGUI _descriptionText;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();

            _button.onClick.AddListener(NextBattle);
        }

        private void NextBattle()
        {
            MainManager.Current.LoadNextBattle();
        }

        public void UpdateDescription(string desc)
        {
            _descriptionText.text = desc;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(NextBattle);
        }
    }
}
