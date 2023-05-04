using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerDeathPopup : MonoBehaviour
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _newRunButton;

        private void Awake()
        {
            _menuButton.onClick.AddListener(MainManager.Current.LoadMenu);
            _newRunButton.onClick.AddListener(MainManager.Current.StartNewRun);
        }
    }
}