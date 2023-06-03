using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ReorderableContent
{
    public class ReorderableGrid : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;
        
        [field: SerializeField] public RectTransform DraggingArea { get; private set; }
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [field: SerializeField] public bool CanGrabElements { get; set; } = true;
        [field: SerializeField] public bool CanDropElements { get; set; } = true;
        [field: SerializeField] public bool IsMergeable { get; set; } = true;

        private List<Transform> _cachedChildren;

        private bool _refreshing = false;
        
        private event Action<List<Transform>> OnRefreshedChildren;
        private event Action OnElementOrderAlteredByDrag;
        
        public RectTransform Content { get; private set; }
        public RectTransform Rect { get; private set; }
        
        public int Size =>
            (int) ((_grid.cellSize.x * Content.childCount) +
                   (_grid.spacing.x * Content.childCount - 1));
        

        private void Awake()
        {
            Content = _grid.GetComponent<RectTransform>();
            Rect = GetComponent<RectTransform>();
        }

        public void Init(IReorderableGridEventListener listener)
        {
            OnRefreshedChildren += listener.ElementsRefreshed;
            OnElementOrderAlteredByDrag += listener.ElementsOrderChangedByDrag;
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (_refreshing) StopCoroutine(RefreshChildren());
            
            _cachedChildren = new List<Transform>();

            _refreshing = true;
            StartCoroutine(RefreshChildren());
        }

        public void ElementOrderAlteredByDrag()
        {
            OnElementOrderAlteredByDrag?.Invoke();
        }

        public void RandomiseChildren()
        {
            _cachedChildren = _cachedChildren.OrderBy(s => Random.value).ToList();

            for (var i = 0; i < _cachedChildren.Count; i++)
            {
                _cachedChildren[i].SetSiblingIndex(i+1);
            }

            Refresh();
        }
        
        private IEnumerator RefreshChildren()
        {
            // Get new children
            for (var i = 0; i < Content.childCount; i++)
            {
                var childTransform = Content.GetChild(i);
                
                if (_cachedChildren.Contains(childTransform))
                    continue;
                
                _cachedChildren.Add(childTransform);
            }

            // A little hack, if we don't wait one frame we don't have the right deleted children
            yield return 0;
            
            // Remove deleted child
            for (var i = _cachedChildren.Count - 1; i >= 0; i--)
            {
                if (_cachedChildren[i] == null)
                {
                    _cachedChildren.RemoveAt(i);
                }
            }
            
            _refreshing = false;
            OnRefreshedChildren?.Invoke(_cachedChildren);
        }
    }
}