using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ReorderableContent;
using Random = UnityEngine.Random;

namespace BattleScreen.BattleBoard
{
    [RequireComponent(typeof(ReorderableGrid))]
    public class Board : MonoBehaviour, IReorderableGridEventListener
    {
        [SerializeField] private Transform _islandTransform;
        [SerializeField] private Transform _boatTransform;

        public static Board Current;
        
        private readonly List<Plank> _cachedPlanks = new List<Plank>();
        
        public ReorderableGrid List { get; private set; }

        public Transform IslandTransform => _islandTransform;
        public Transform BoatTransform => _boatTransform;

        public int BoardSize => List.Size;

        public int PlankCount => _cachedPlanks.Count;
        public List<Plank> PlanksInOrder => _cachedPlanks.OrderBy(p => p.PlankNum).ToList();


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
                (!Player.Current.HasMovesLimit || !Player.Current.IsOutOfMoves);
        }

        public void ElementsOrderChangedByDrag()
        {
            Debug.Log("Player Moved Plank");
            Battle.Current.InvokeResponsesToPlayerTurnEvent(new BattleEvent(BattleEventType.PlayerMovedPlank));
        }

        public void ElementsRefreshed(List<Transform> listElements)
        {
            var planksInList = listElements.Select(t => t.GetComponentInChildren<Plank>()).ToList();

            foreach (var plank in planksInList.Where(plank => !_cachedPlanks.Contains(plank)))
            {
                plank.Init(this);
                _cachedPlanks.Add(plank);
            }

            foreach (var plank in _cachedPlanks.Where(plank => !planksInList.Contains(plank)))
            {
                _cachedPlanks.Remove(plank);
            }
        }

        public void Refresh()
        {
            List.Refresh();
        }
    }
}