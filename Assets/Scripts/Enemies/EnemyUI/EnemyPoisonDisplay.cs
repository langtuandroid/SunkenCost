using TMPro;
using UnityEngine;

namespace Enemies.EnemyUI
{
    public class EnemyPoisonDisplay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _poisonText;

        public void UpdateDisplay(int poisonAmount)
        {
            _canvasGroup.alpha = poisonAmount > 0 ? 1 : 0;
            _poisonText.text = poisonAmount.ToString();
        }
    }
}