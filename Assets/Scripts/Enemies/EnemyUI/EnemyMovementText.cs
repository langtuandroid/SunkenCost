using TMPro;
using UnityEngine;

namespace Enemies.EnemyUI
{
    public class EnemyMovementText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        
        public void UpdateMovementText(int moveAmount)
        {
            var movement = moveAmount.ToString();
            switch (moveAmount)
            {
                case <0:
                    movement = "<sprite=0> " + movement;
                    break;
                case 0:
                    movement = "-";
                    break;
                case >0:
                    movement += "<sprite=1>";
                    break;
            }

            _textMeshProUGUI.text = movement;
        }
    }
}