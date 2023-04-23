using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace Enemies.EnemyUI
{
    public class EnemyPositioner : MonoBehaviour 
    {
        public const int EnemyOffset = 100;
        private const float Smooth = 0.01f;
        
        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;

        private bool _running = false;

        public void Move(Vector2 aimPosition)
        {
            if (_running) StopAllCoroutines();
            StartCoroutine(VisualiseMove(aimPosition));
        }

        private IEnumerator VisualiseMove(Vector2 aimPosition)
        {
            _running = true;
            
            while (Vector3.Distance(transform.localPosition, aimPosition) > 0.01f)
            {
                var localPosition = transform.localPosition;
                var newPositionX = Mathf.SmoothDamp(
                    localPosition.x, aimPosition.x, ref _moveVelocityX, Smooth);
                var newPositionY = Mathf.SmoothDamp(
                    localPosition.y, aimPosition.y, ref _moveVelocityY, Smooth);
                transform.localPosition = new Vector3(newPositionX, newPositionY, 0);
                yield return new WaitForSeconds(0.01f);
            }

            _running = false;
        }
    }
}