using System;
using TMPro;
using UnityEngine;

namespace OfferScreen
{
    public class MovesDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void Awake()
        {
            UpdateText(RunProgress.PlayerProgress.MovesPerTurn);
        }

        private void UpdateText(int moves)
        {
            textMeshProUGUI.text = moves.ToString();
        }
    }
}