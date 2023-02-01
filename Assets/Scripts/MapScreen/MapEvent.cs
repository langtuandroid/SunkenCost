using UnityEngine;
using UnityEngine.UI;

namespace MapScreen
{

    public enum MapEventType
    {
        Coin,
        Heart,
        UpgradeCard
    }

    public class MapEvent : MonoBehaviour
    {
        private Button _button;

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
