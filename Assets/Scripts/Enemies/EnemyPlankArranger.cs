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

        private const float IslandXOffset = 200f;
        private const float IslandYOffset = -400f;
        private const float PlankXOffset = 0f;
        private const float PlankYOffset = -425f;
        private const float BoatXOffset = 15f;
        private const float BoatYOffset = -400f;
        
        private static float TweenTime => Battle.ActionExecutionSpeed / 5f;

        private void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
        }
        
        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EnemySpawned && !battleEvent.showResponse)
            {
                ArrangeEnemiesOnPlank(battleEvent.Enemy.PlankNum, true);
            }
            /*
            else if (battleEvent.type == BattleEventType.EnemyReachedBoat)
            {
                var enemy = BattleEventsManager.Current.GetEnemyByResponderID(battleEvent.affectedResponderID);
                var aimPosition = new Vector2(BoatXOffset, BoatYOffset);
                StartCoroutine(MoveTransform(Board.Current.BoatTransform, enemy, aimPosition));
            }
            */
            else if (battleEvent.type == BattleEventType.EnemyMove ||
               battleEvent.type == BattleEventType.EnemySpawned)
            {
                ArrangeAllEnemies();
            }
            else if (battleEvent.type == BattleEventType.EnemyKilled && battleEvent.source != DamageSource.Boat)
            {
                StartCoroutine(WaitForEnemyDeath(battleEvent));
            }
            
        }

        private void ArrangeAllEnemies()
        {
            for (var i = -1; i < Board.Current.PlankCount; i++) ArrangeEnemiesOnPlank(i, false);
        }

        private void ArrangeEnemiesOnPlank(int plankNum, bool doImmediately)
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
                if (doImmediately)
                    enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(aimPosition.x, aimPosition.y);
                else
                    MoveTransform(plankTransform, enemy, aimPosition);
            }
        }

        private IEnumerator WaitForEnemyDeath(BattleEvent battleEvent)
        {
            var plankNum = battleEvent.Enemy.PlankNum;

            // Wait for just a little bit shorter time than the game logic
            var waitTime = Battle.ActionExecutionSpeed * Battle.GetAnimationTime(battleEvent) - 0.01f;
            
            yield return new WaitForSecondsRealtime(waitTime);
            ArrangeEnemiesOnPlank(plankNum, false);
        }

        private void MoveTransform(Transform plankTransform, Enemy enemy, Vector2 aimPosition)
        {
            var enemyRectTransform = enemy.GetComponent<RectTransform>();
            enemyRectTransform.SetParent(plankTransform);
            enemyRectTransform.SetAsLastSibling();
            
            LeanTween.cancel(enemyRectTransform);
            LeanTween.move(enemyRectTransform, aimPosition, TweenTime);
        }
    }
}