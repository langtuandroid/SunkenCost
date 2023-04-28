using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Enemies.EnemyUI;
using UnityEngine;

namespace Enemies
{
    public class EnemyPlankArranger : MonoBehaviour, IBattleEventUpdatedUI
    {
        private const int EnemyDefaultSize = 125;
        private const float Smooth = 0.01f;

        private const float IslandXOffset = 200f;
        private const float IslandYOffset = -400f;
        private const float PlankXOffset = 0f;
        private const float PlankYOffset = -400f;
        
        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;

        private void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
        }
        
        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EnemyMove || battleEvent.type == BattleEventType.EnemyKilled ||
                battleEvent.type == BattleEventType.EnemySpawned)
                UpdatePosition(battleEvent.enemyAffectee.PlankNum);
        }

        public void UpdatePosition(int plankNum)
        {
            var isOnIsland = plankNum == -1;
            var plankTransform = isOnIsland ? Board.Current.Island : Board.Current.Content.GetChild(plankNum);

            var enemiesOnPlank = EnemySequencer.Current.AllEnemies
                .Where(e => !e.IsDestroyed && e.PlankNum == plankNum);
            var enemiesInOrder = enemiesOnPlank.OrderBy(e => e.TurnOrder).ToArray();
            
            var baseXOffset = isOnIsland ? IslandXOffset : PlankXOffset;
            var baseYOffset = isOnIsland ? IslandYOffset : PlankYOffset;

            var offsetFromEnemiesAbove = 0f;

            foreach (var enemy in enemiesInOrder)
            {
                var enemyRectTransform = enemy.GetComponent<RectTransform>();
                enemyRectTransform.SetParent(plankTransform);

                var yOffset = baseYOffset - offsetFromEnemiesAbove;
                offsetFromEnemiesAbove += enemy.Size * EnemyDefaultSize;
                
                var aimPosition = new Vector2(baseXOffset, yOffset);
                Debug.Log("Moving " + enemy.Name + " to (" + aimPosition.x + ", " + aimPosition.y + ")");
                StartCoroutine(MoveTransform(enemyRectTransform, aimPosition));
            }
        }

        private IEnumerator MoveTransform(RectTransform enemyRectTransform, Vector2 aimPosition)
        {
            while (Vector3.Distance(enemyRectTransform.anchoredPosition, aimPosition) > 0.01f)
            {
                var anchoredPosition = enemyRectTransform.anchoredPosition;
                var newPositionX = Mathf.SmoothDamp(
                    anchoredPosition.x, aimPosition.x, ref _moveVelocityX, Smooth);
                var newPositionY = Mathf.SmoothDamp(
                    anchoredPosition.y, aimPosition.y, ref _moveVelocityY, Smooth);
                enemyRectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}