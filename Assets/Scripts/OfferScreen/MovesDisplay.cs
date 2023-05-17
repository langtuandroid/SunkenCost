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
            UpdateText(RunProgress.Current.PlayerStats.MovesPerTurn);
        }

        private void UpdateText(int moves)
        {
            textMeshProUGUI.text = moves.ToString();
        }
    }
}