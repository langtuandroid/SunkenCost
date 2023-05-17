using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerDeathPopup : MonoBehaviour
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _newRunButton;
        [SerializeField] private TextMeshProUGUI _detailsText;

        private void Awake()
        {
            _menuButton.onClick.AddListener(MainManager.Current.LoadMenu);
            _newRunButton.onClick.AddListener(MainManager.Current.StartNewRun);

            var text = _detailsText.text;
            var updatedText = text.Replace("@", RunProgress.Current.BattleNumber.ToString());
            _detailsText.text = updatedText;
        }
    }
}