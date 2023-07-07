using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Etchings;
using ReorderableContent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleScreen.BattleBoard
{
    [RequireComponent(typeof(PlankDisplay))]
    [RequireComponent(typeof(ReorderableElement))]
    public class Plank : MonoBehaviour
    {
        private PlankDisplay _display;
        public Etching Etching { get; private set; }
        public int PlankNum => transform.GetSiblingIndex();

        private void Awake()
        {
            _display = GetComponent<PlankDisplay>();
        }

        public void Init(Board board)
        {
            var mergeableListElement = GetComponent<ReorderableElement>();
            mergeableListElement.SetListener(_display);
        }

        public void SetEtching(Etching etching)
        {
            Etching = etching;
        }

        public List<BattleEvent> Destroy(DamageSource source)
        {
            var response = new List<BattleEvent>();

            foreach (var enemy in EnemySequencer.Current.GetEnemiesOnPlank(PlankNum))
            {
                response.AddRange(enemy.KillImmediately(DamageSource.PlankDestruction).battleEvents);
            }
            
            response.Add(new BattleEvent(BattleEventType.PlankDestroyed) {modifier = PlankNum, source = source});
            Destroy(gameObject);
            return response;
        }
    }
}