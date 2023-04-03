using TMPro;
using UnityEngine;

namespace Enemies.EnemyUI
{
    public class EnemyMovementText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        
        public void UpdateMovementText(MovementType movementType, int amountOfMovesLeft)
        {
            var movement = amountOfMovesLeft.ToString();

            var spriteIndexModifer = movementType == MovementType.Walk ? 0 : 2;
            
            switch (amountOfMovesLeft)
            {
                case <0:
                    movement = "<sprite=" + spriteIndexModifer + "> " + Mathf.Abs(amountOfMovesLeft);
                    break;
                case 0:
                    movement = "-";
                    break;
                case >0:
                    movement += "<sprite=" + (spriteIndexModifer + 1) + ">";
                    break;
            }

            _textMeshProUGUI.text = movement;
        }
    }
}