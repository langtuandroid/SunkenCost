﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReorderableContent
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class ReorderableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private ReorderableGrid _currentReorderableGrid;

        private RectTransform _rectTransform;

        private int _currentSiblingIndex;
        private ReorderableGrid _listHoveringOver;
        private RectTransform _placeholderRect;

        private RectTransform _fakeRectTransform;

        private bool _justSpawnedFake;
        private bool _isDragging;
        private bool _isHoveringOverReorderableElement;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();
        private ReorderableElement _elementToMergeWith;

        private event Action OnStartedDrag;
        private event Action<ReorderableGrid> OnHoveringOverList;
        private event Action OnEndedDrag;

        private event Action<ReorderableElement> OnOfferMerge;
        private event Action OnCancelMerge;
        private event Action OnFinaliseMerge;

        private bool _currentlyMerging;
        private Func<ReorderableElement, bool> TryMergeInto;
        
        [field: SerializeField] public bool IsMergeable { get; private set; }
        public bool HasBeenInitialised { get; private set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Init(ReorderableGrid reorderableGrid)
        {
            HasBeenInitialised = true;
            
            _canvas = reorderableGrid.Canvas;
            _currentReorderableGrid = reorderableGrid;
            _currentSiblingIndex = transform.GetSiblingIndex();

            _placeholderRect = CreatePlaceholderRect();
            
            IsMergeable = reorderableGrid.IsMergeable;
        }

        private void OnEnable()
        {
            if (_currentReorderableGrid && !_placeholderRect)
                _placeholderRect = CreatePlaceholderRect();
        }

        private void OnDisable()
        {
            if (_placeholderRect)
                Destroy(_placeholderRect.gameObject);
            if (_fakeRectTransform) Destroy(_fakeRectTransform.gameObject);
        }

        public void SetListener(IReorderableElementEventListener listener = null)
        {
            if (listener != null)
            {
                OnStartedDrag += listener.Grabbed;
                OnHoveringOverList += listener.HoveringOverList;
                OnEndedDrag += listener.Released;

                if (IsMergeable)
                {
                    if (listener is IMergeableReorderableEventListener mergeableListener)
                    {
                        TryMergeInto = mergeableListener.GetIfCanMerge();
                        OnOfferMerge += mergeableListener.OfferMerge;
                        OnCancelMerge += mergeableListener.CancelMerge;
                        OnFinaliseMerge += mergeableListener.FinaliseMerge;
                    }
                    else
                    {
                        throw new Exception($"Listener {listener.GetType().Name} is not a mergeable listener!");
                    }
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !_currentReorderableGrid.CanGrabElements) return;

            _isDragging = true;
            _canvasGroup.blocksRaycasts = false;
            OnStartedDrag?.Invoke();

            _currentSiblingIndex = _rectTransform.GetSiblingIndex();
            _listHoveringOver = _currentReorderableGrid;
            
            // Move this plank out of the content area
            _rectTransform.SetParent(_currentReorderableGrid.DraggingArea);
            _rectTransform.SetAsLastSibling();
            
            // Create an empty space where the current element is in the placeholder grid
            _fakeRectTransform = CreateEmptySpace(_currentReorderableGrid.Content, _currentSiblingIndex,$"Empty Space for {GetInstanceID()}");

            _justSpawnedFake = true;
            
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;
            
            // Little hack - wait one frame for the empty space object to register
            if (_justSpawnedFake)
            {
                _justSpawnedFake = false;
                return;
            }

            // Move to cursor
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas.GetComponent<RectTransform>(), 
                eventData.position, _canvas.renderMode != RenderMode.ScreenSpaceOverlay
                    ? _canvas.worldCamera : null, out var worldPoint);
            _rectTransform.position = worldPoint;
            
            // Check everything under the cursor to find a MergeableList
            EventSystem.current.RaycastAll(eventData, _raycastResults);
            
            _listHoveringOver = _raycastResults
                .Select(r => r.gameObject.GetComponent<ReorderableGrid>())
                .FirstOrDefault(r => r is not null);

            if (IsMergeable && _listHoveringOver is not null)
            {
                var oldElement = _elementToMergeWith;
                _elementToMergeWith = _raycastResults
                    .Select(r => r.gameObject.GetComponent<ReorderableElement>())
                    .FirstOrDefault(r => r is not null && r != this && r.IsMergeable && TryMergeInto.Invoke(r));

                if (_elementToMergeWith is not null)
                {
                    if (oldElement == _elementToMergeWith) return;
                    
                    // Hovered straight over mergeable item as soon as entered grid
                    if (_listHoveringOver != _currentReorderableGrid)
                    {
                        Assert.IsNotNull(_listHoveringOver);
                        _currentReorderableGrid = _listHoveringOver;
                        OnHoveringOverList?.Invoke(_listHoveringOver);
                    }
                    
                    _currentlyMerging = true;
                    OnOfferMerge?.Invoke(_elementToMergeWith);
                    _fakeRectTransform.SetParent(_currentReorderableGrid.DraggingArea, true);
                    _placeholderRect.SetParent(_currentReorderableGrid.DraggingArea, true);
                    return;
                }
                
                if (_currentlyMerging)
                {
                    _currentlyMerging = false;
                    OnCancelMerge?.Invoke();
                }
            }
            
            if ((_listHoveringOver == null || !_listHoveringOver.CanDropElements))
            {
                if (_currentReorderableGrid.CanRemoveElementsFromGrid)
                {
                    // Nothing found or not droppable - put the fake element outside of all lists
                    _fakeRectTransform.SetParent(_currentReorderableGrid.DraggingArea, true);
                    _placeholderRect.SetParent(_currentReorderableGrid.DraggingArea, true);
                }
                
                return;
            }
            
            _currentReorderableGrid = _listHoveringOver;

            
            // Update the parent if we've changed lists
            if (_fakeRectTransform.parent != _listHoveringOver.Content)
            {
                OnHoveringOverList?.Invoke(_listHoveringOver);
                _placeholderRect.SetParent(_listHoveringOver.PlaceholderContent, true);
                _fakeRectTransform.SetParent(_listHoveringOver.Content, true);
            }

            // Put the empty space in the right place
            var distanceOfClosestElement = float.PositiveInfinity;
            var closestPlankSiblingIndex = 0;
            
            for (var i = 0; i < _listHoveringOver.PlaceholderContent.childCount; i++)
            {
                var rectTransform = _listHoveringOver.PlaceholderContent.GetChild(i).GetComponent<RectTransform>();
                var rectPosition = rectTransform.position;
                var distance = Mathf.Abs
                    (rectPosition.x - worldPoint.x) + Mathf.Abs(rectPosition.y - worldPoint.y);

                if (distance < distanceOfClosestElement)
                {
                    distanceOfClosestElement = distance;
                    closestPlankSiblingIndex = i;
                }
            }

            _placeholderRect.SetSiblingIndex(closestPlankSiblingIndex);
            _fakeRectTransform.SetSiblingIndex(closestPlankSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging || eventData.button != PointerEventData.InputButton.Left) return;

            if (_currentlyMerging)
            {
                OnFinaliseMerge?.Invoke();
                Destroy(gameObject);
                return;
            }
            
            _isDragging = false;
            OnEndedDrag?.Invoke();

            var oldList = _currentReorderableGrid;
            var oldSiblingIndex = _currentSiblingIndex;

            if (_listHoveringOver != null)
            {
                _currentReorderableGrid = _listHoveringOver;
                _currentSiblingIndex = _placeholderRect.GetSiblingIndex();
            }
            
            _rectTransform.SetParent(_currentReorderableGrid.Content, true);
            _placeholderRect.SetParent(_currentReorderableGrid.PlaceholderContent, true);
            _placeholderRect.SetSiblingIndex(_currentSiblingIndex);
            _rectTransform.SetSiblingIndex(_currentSiblingIndex);
            Destroy(_fakeRectTransform.gameObject);
            
            _canvasGroup.blocksRaycasts = true;

            if (_listHoveringOver != null && (oldSiblingIndex != _currentSiblingIndex || _currentReorderableGrid != oldList))
            {
                _currentReorderableGrid.ElementOrderAlteredByDrag();
            }
        }

        public void Reposition()
        {
            if (_currentlyMerging) return;
            
            var lerpObject = _isDragging ? _fakeRectTransform.gameObject : gameObject;
            
            LeanTween.cancel(lerpObject);
            LeanTween.moveLocal(lerpObject, _placeholderRect.localPosition, 0.1f);
        }

        public void SetSiblingIndex(int i)
        {
            _placeholderRect.SetSiblingIndex(i);
        }

        private RectTransform CreatePlaceholderRect()
        {
            return CreateEmptySpace(_currentReorderableGrid.PlaceholderContent, _currentSiblingIndex,
                $"Placeholder for {GetInstanceID()}");
        }
        
        private RectTransform CreateEmptySpace(RectTransform listContent, int siblingIndex, string gameObjectName)
        {
            // Create an empty space where the current element is in the placeholder grid
            var emptySpace = new GameObject(gameObjectName);
            var emptySpaceRect = emptySpace.AddComponent<RectTransform>();
            emptySpaceRect.SetParent(listContent, false);
            emptySpaceRect.SetSiblingIndex(siblingIndex);
            emptySpaceRect.sizeDelta = _rectTransform.sizeDelta;

            return emptySpaceRect;
        }
    }
}