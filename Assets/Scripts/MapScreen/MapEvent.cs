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

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();

            _button.onClick.AddListener(NextBattle);
        }

        private void NextBattle()
        {
            MainManager.Current.LoadNextBattle();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(NextBattle);
        }
    }
}
