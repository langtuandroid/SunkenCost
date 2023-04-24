using System;
using System.Collections;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace Enemies.EnemyUI
{
    public class EnemyTransformPositioner : MonoBehaviour 
    {
        private const int EnemyDefaultSize = 125;
        private const float Smooth = 0.01f;
        
        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;

        private bool _running = false;

        private RectTransform _rectTransform;
        private Enemy _enemy;
        
        public int TurnOrder { get; private set; }

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.SetAsLastSibling();
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = GetAnchoredPosition(_rectTransform.parent, _enemy.PlankNum == -1);
        }

        public void UpdatePosition(int plankNum, int turnOrder)
        {
            if (_running) StopAllCoroutines();
            TurnOrder = turnOrder;
            StartCoroutine(VisualiseMove(plankNum));
        }

        private IEnumerator VisualiseMove(int plankNum)
        {
            _running = true;
            
            var isOnIsland = plankNum == -1;
            var parentTransform = isOnIsland ? Board.Current.Island : Board.Current.Content.GetChild(plankNum);
            
            _rectTransform.SetParent(parentTransform);

            var aimPosition = GetAnchoredPosition(parentTransform, isOnIsland);

            while (Vector3.Distance(_rectTransform.anchoredPosition, aimPosition) > 0.01f)
            {
                var anchoredPosition = _rectTransform.anchoredPosition;
                var newPositionX = Mathf.SmoothDamp(
                    anchoredPosition.x, aimPosition.x, ref _moveVelocityX, Smooth);
                var newPositionY = Mathf.SmoothDamp(
                    anchoredPosition.y, aimPosition.y, ref _moveVelocityY, Smooth);
                _rectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY, 0);
                yield return new WaitForSeconds(0.01f);
            }

            _running = false;
        }

        private Vector2 GetAnchoredPosition(Transform parentTransform, bool isOnIsland)
        {
            var otherEnemiesOnTransform = 
                parentTransform.GetComponentsInChildren<Enemy>()
                    .Where(e => e.GetComponent<EnemyTransformPositioner>().TurnOrder < TurnOrder);
            
            var xOffset = isOnIsland ? 200f : 0f;
            var yOffset = isOnIsland ? -400f : -400f;

            // Offset based off the size of the other enemies - ignore this enemy
            yOffset -= otherEnemiesOnTransform
                .Sum(enemy => enemy.Size * EnemyDefaultSize);
            
            return new Vector2(xOffset, yOffset);
        }
    }
}