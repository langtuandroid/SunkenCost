using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleBoard;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ReorderableContent
{
    public class ReorderableGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;
        [field: SerializeField] public RectTransform Content { get; private set; }
        
        [field: SerializeField] public RectTransform DraggingArea { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public bool CanDropElements { get; set; } = true;
        [field: SerializeField] public bool IsMergeable { get; set; } = true;
        
        
        public bool CanGrabElements => _canGrabElements.Invoke();

        private List<ReorderableElement> _cachedElements;

        private bool _refreshing = false;

        private Func<bool> _canGrabElements = () => true;
        
        private event Action<List<ReorderableElement>> OnRefreshedChildren;
        private event Action OnElementOrderAlteredByDrag;
        public RectTransform Rect { get; private set; }
        
        public int Size =>
            (int) ((_grid.cellSize.x * Content.childCount) +
                   (_grid.spacing.x * Content.childCount - 1));

        public RectTransform PlaceholderContent { get; private set; }
        

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
            PlaceholderContent = _grid.GetComponent<RectTransform>();
        }

        public void Init(IReorderableGridEventListener listener)
        {
            _canGrabElements = listener.CanGrabElementsFunc();
            OnRefreshedChildren += listener.ElementsRefreshed;
            OnElementOrderAlteredByDrag += listener.ElementsOrderChangedByDrag;
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void LerpElements()
        {
            foreach (var element in _cachedElements)
            {
                element.Reposition();
            }
        }

        public void Refresh()
        {
            if (_refreshing) StopCoroutine(RefreshChildren());
            
            _cachedElements = new List<ReorderableElement>();

            _refreshing = true;
            StartCoroutine(RefreshChildren());
        }

        public void ElementOrderAlteredByDrag()
        {
            OnElementOrderAlteredByDrag?.Invoke();
        }

        public void RandomiseChildren()
        {
            _cachedElements = _cachedElements.OrderBy(s => Random.value).ToList();

            for (var i = 0; i < _cachedElements.Count; i++)
            {
                _cachedElements[i].SetSiblingIndex(i+1);
            }

            Refresh();
        }
        
        private IEnumerator RefreshChildren()
        {
            // Get new children
            for (var i = 0; i < Content.childCount; i++)
            {
                var element = Content.GetChild(i).GetComponent<ReorderableElement>();
                
                if (_cachedElements.Contains(element))
                    continue;
                
                _cachedElements.Add(element);
            }

            // A little hack, if we don't wait one frame we don't have the right deleted children
            yield return 0;
            yield return 0;
            
            // Remove deleted child
            for (var i = _cachedElements.Count - 1; i >= 0; i--)
            {
                if (_cachedElements[i] == null)
                {
                    _cachedElements.RemoveAt(i);
                    continue;
                }
            }
            Debug.Log(_cachedElements.Count);
            _refreshing = false;
            LerpElements();
            OnRefreshedChildren?.Invoke(_cachedElements);
        }
    }
}