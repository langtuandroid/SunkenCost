using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Damage;
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
            mergeableListElement.Init(board.List, _display);
        }

        public void SetEtching(Etching etching)
        {
            Etching = etching;
        }

        public BattleEvent Destroy(DamageSource source)
        {
            Destroy(gameObject);
            return new BattleEvent(BattleEventType.PlankDestroyed);
        }
    }
}