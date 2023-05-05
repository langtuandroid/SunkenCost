using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;
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
        private const float BoatXOffset = 0f;
        private const float BoatYOffset = -400f;
        
        private float _moveVelocityX = 0f;
        private float _moveVelocityY = 0f;

        private void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
        }
        
        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EnemyReachedBoat)
            {
                var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
                var aimPosition = new Vector2(BoatXOffset, BoatYOffset);
                StartCoroutine(MoveTransform(Board.Current.BoatTransform, enemy, aimPosition));
            }
            else if (battleEvent.type == BattleEventType.EnemyMove ||
               (battleEvent.type == BattleEventType.EnemyKilled && battleEvent.source != DamageSource.Boat) ||
               battleEvent.type == BattleEventType.EnemySpawned)
            {
                var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
                UpdatePosition(enemy.PlankNum);
            }
            
        }

        private void UpdatePosition(int plankNum)
        {
            var isOnIsland = plankNum == -1;

            var enemiesOnPlank = EnemySequencer.Current.AllEnemies
                .Where(e => !e.IsDestroyed && e.PlankNum == plankNum);
            var enemiesInOrder = enemiesOnPlank.OrderBy(e => e.TurnOrder).ToArray();
            
            var baseXOffset = isOnIsland ? IslandXOffset : PlankXOffset;
            var baseYOffset = isOnIsland ? IslandYOffset : PlankYOffset;

            var offsetFromEnemiesAbove = 0f;

            var plankTransform = isOnIsland ? Board.Current.IslandTransform : Board.Current.Content.GetChild(plankNum);
            
            foreach (var enemy in enemiesInOrder)
            {
                var yOffset = baseYOffset - offsetFromEnemiesAbove;
                offsetFromEnemiesAbove += enemy.Size * EnemyDefaultSize;
                
                var aimPosition = new Vector2(baseXOffset, yOffset);
                //Debug.Log("Moving " + enemy.Name + " to (" + aimPosition.x + ", " + aimPosition.y + ")");
                StartCoroutine(MoveTransform(plankTransform, enemy, aimPosition));
            }
        }

        private IEnumerator MoveTransform(Transform plankTransform, Enemy enemy, Vector2 aimPosition)
        {
            var enemyRectTransform = enemy.GetComponent<RectTransform>();
            enemyRectTransform.SetParent(plankTransform);
            
            while (Vector3.Distance(enemyRectTransform.anchoredPosition, aimPosition) > 0.01f)
            {
                var anchoredPosition = enemyRectTransform.anchoredPosition;
                var newPositionX = Mathf.SmoothDamp(
                    anchoredPosition.x, aimPosition.x, ref _moveVelocityX, Smooth);
                var newPositionY = Mathf.SmoothDamp(
                    anchoredPosition.y, aimPosition.y, ref _moveVelocityY, Smooth);
                enemyRectTransform.anchoredPosition = new Vector3(newPositionX, newPositionY, 0);
                yield return new WaitForSecondsRealtime(0.01f);
                
                if (enemy.IsDestroyed) yield break;
            }
        }
    }
}