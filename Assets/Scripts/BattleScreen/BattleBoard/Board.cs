using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScreen.BattleBoard
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _plankArea;
        [SerializeField] private Transform _island;

        public static Board Current;
        
        private List<Transform> _cachedChildren;
        private List<Plank> _cachedPlanks;
        
        private bool _refreshing = false;
        
        public Canvas Canvas { get; private set; }
        public RectTransform Content { get; private set; }
        public RectTransform Rect { get; private set; }

        public Transform Island => _island;
        
        public bool CanMovePlanks => !Player.Current.IsOutOfMoves && Battle.Current.GameState == GameState.PlayerTurn;
        
        public int PlankCount => _cachedPlanks.Count;
        public List<Plank> PlanksInOrder => _cachedPlanks.OrderBy(p => p.transform.GetSiblingIndex()).ToList();

        public int BoardSize =>
            (int) ((_plankArea.cellSize.x * Content.childCount) +
                   (_plankArea.spacing.x * Content.childCount - 1));

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
            
            Canvas = GetComponentInParent<Canvas>();
            Content = _plankArea.GetComponent<RectTransform>();
            Rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (_refreshing) StopCoroutine(RefreshChildren());
            
            _cachedChildren = new List<Transform>();
            _cachedPlanks = new List<Plank>();

            _refreshing = true;
            StartCoroutine(RefreshChildren());
        }

        public void RandomisePlanks()
        {
            _cachedChildren = _cachedChildren.OrderBy(s => Random.value).ToList();

            for (var i = 0; i < _cachedChildren.Count; i++)
            {
                _cachedChildren[i].SetSiblingIndex(i+1);
            }

            Refresh();
        }
        
        public Plank GetPlank(int position)
        {
            return PlanksInOrder[position];
        }

        private IEnumerator RefreshChildren()
        {
            // Get new children
            for (var i = 0; i < Content.childCount; i++)
            {
                var childTransform = Content.GetChild(i);
                
                if (_cachedChildren.Contains(childTransform))
                    continue;

                var newPlank = childTransform.GetComponent<Plank>();
                newPlank.Init(this);
                
                _cachedChildren.Add(childTransform);
                _cachedPlanks.Add(newPlank);
            }

            // A little hack, if we don't wait one frame we don't have the right deleted children
            yield return 0;
            
            // Remove deleted child
            for (var i = _cachedChildren.Count - 1; i >= 0; i--)
            {
                if (_cachedChildren[i] == null)
                {
                    _cachedChildren.RemoveAt(i);
                    _cachedPlanks.RemoveAt(i);
                }
            }
            
            _refreshing = false;
        }
    }
}