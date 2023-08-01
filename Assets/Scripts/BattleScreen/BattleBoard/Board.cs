using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Damage;
using UnityEngine;
using UnityEngine.UI;
using ReorderableContent;
using Random = UnityEngine.Random;

namespace BattleScreen.BattleBoard
{
    [RequireComponent(typeof(ReorderableGrid))]
    public class Board : MonoBehaviour, IReorderableGridEventListener
    {
        [SerializeField] private RectTransform _islandRectTransform;
        [SerializeField] private RectTransform _boatRectTransform;

        public static Board Current;
        
        private readonly List<Plank> _cachedPlanks = new List<Plank>();
        
        public ReorderableGrid List { get; private set; }

        public RectTransform IslandTransform => _islandRectTransform;
        public RectTransform BoatTransform => _boatRectTransform;

        public int BoardSize => List.Size;

        public int PlankCount => _cachedPlanks.Count;
        public List<Plank> PlanksInOrder => _cachedPlanks.Where(p => p).OrderBy(p => p.PlankNum).ToList();

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
            List = GetComponent<ReorderableGrid>();
            List.Init(this);
        }
        
        public Plank GetPlank(int position)
        {
            Debug.Log("Planks: " + String.Join(", ", PlanksInOrder.Select(p => p.Etching.Design.Title + " " + p.PlankNum)));
            return PlanksInOrder[position];
        }

        public void RandomisePlanks()
        {
            List.RandomiseChildren();
        }

        public Func<bool> CanGrabElementsFunc()
        {
            return () => 
                Battle.Current.BattleState == BattleState.PlayerActionPeriod &&
                (!Player.Current.MoveLimit.HasValue || !Player.Current.IsOutOfMoves);
        }

        public void ElementsOrderChangedByDrag()
        {
            Debug.Log("Player Moved Plank");
            Battle.Current.InvokeResponsesToPlayerTurnEvent(new BattleEvent(BattleEventType.PlayerMovedPlank));
        }

        public void ElementsRefreshed(List<ReorderableElement> listElements)
        {
            var planksInList = listElements.Select(t => t.GetComponent<Plank>()).ToList();

            foreach (var plank in planksInList.Where(plank => !_cachedPlanks.Contains(plank)))
            {
                plank.Init(this);
                _cachedPlanks.Add(plank);
            }

            for (var i = _cachedPlanks.Count - 1; i >= 0; i--)
            {
                if (_cachedPlanks[i] is null || !planksInList.Contains((_cachedPlanks[i])))
                {
                    _cachedPlanks.RemoveAt(i);
                }
            }
            
            Debug.Log($"Refreshed with {string.Join(", ",_cachedPlanks.Select(c => c.Etching.Design.Title))}");
        }
        
        public List<BattleEvent> DestroyAllPlanks()
        {
            var response = new List<BattleEvent>();

            for (var i = PlanksInOrder.Count - 1; i >= 0; i--)
            {
                response.AddRange(PlanksInOrder[i].Destroy(DamageSource.PlankDestruction));
            }
            
            return response;
        }
    }
}