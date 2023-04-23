using Etchings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleScreen.BattleBoard
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(PlankDisplay))]
    public class Plank : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas _canvas;
        private BattleBoard.Board _board;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private PlankDisplay _display;
        private LayoutElement _layoutElement;

        private int _lastSiblingIndex;
        private RectTransform _emptySpaceRect;

        private bool _justSpawnedFake = false;

        private bool _isDragging;
        
        public Etching Etching { get; private set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _display = GetComponent<PlankDisplay>();
            _layoutElement = gameObject.AddComponent<LayoutElement>();
            
            var rect = _rectTransform.rect;
            _layoutElement.preferredWidth = rect.size.x;
            _layoutElement.preferredHeight = rect.size.y;
        }

        public void Init(BattleBoard.Board board)
        {
            _board = board;
        }

        private void Start()
        {
            Etching = GetComponentInChildren<Etching>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !_board.CanMovePlanks) return;

            _isDragging = true;
            _canvasGroup.blocksRaycasts = false;
            _display.BeginDrag();

            _lastSiblingIndex = _rectTransform.GetSiblingIndex();

            
            // Create an empty space where the current plank is
            var emptySpace = new GameObject("Empty space");
            _emptySpaceRect = emptySpace.AddComponent<RectTransform>();
            _emptySpaceRect.SetParent(_board.Content, false);
            _emptySpaceRect.SetSiblingIndex(_lastSiblingIndex);
            _emptySpaceRect.sizeDelta = _rectTransform.sizeDelta;
            emptySpace.AddComponent<LayoutElement>();
            
            // Move out of the content area
            _rectTransform.SetParent(_board.Rect);
            _rectTransform.SetAsLastSibling();

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

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_board.Canvas.GetComponent<RectTransform>(), 
                eventData.position, _board.Canvas.renderMode != RenderMode.ScreenSpaceOverlay
                    ? _board.Canvas.worldCamera : null, out var worldPoint);
            _rectTransform.position = worldPoint;

            var distanceOfClosestPlank = float.PositiveInfinity;
            var closestPlankSiblingIndex = 0;

            for (var i = 0; i < _board.Content.childCount; i++)
            {
                var rectPosition = _board.Content.GetChild(i).GetComponent<RectTransform>().position;
                var distance = Mathf.Abs
                    (rectPosition.x - worldPoint.x) + Mathf.Abs(rectPosition.y - worldPoint.y);

                if (distance < distanceOfClosestPlank)
                {
                    distanceOfClosestPlank = distance;
                    closestPlankSiblingIndex = i;
                }
            }
            
            _emptySpaceRect.SetSiblingIndex(closestPlankSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging || eventData.button != PointerEventData.InputButton.Left) return;

            _isDragging = false;
            _display.EndDrag();

            _rectTransform.SetParent(_board.Content, false);
            var newSiblingIndex = _emptySpaceRect.GetSiblingIndex();
            _rectTransform.SetSiblingIndex(newSiblingIndex);

            Destroy(_emptySpaceRect.gameObject);
            _canvasGroup.blocksRaycasts = true;
            
            if (_lastSiblingIndex != newSiblingIndex)
            {
                PlayerTurnEvents.Current.InvokeEvent(new BattleEvent(BattleEventType.PlayerMovedPlank));
            }
        }
        
        public void Destroy(DamageSource source)
        {
            var enemies = EnemyController.Current.GetEnemiesOnPlank(transform.GetSiblingIndex());

            foreach (var enemy in enemies)
            {
                enemy.DestroySelf(source);
            }
            Destroy(gameObject);
        }
    }
}