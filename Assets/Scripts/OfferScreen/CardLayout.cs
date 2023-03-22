using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace OfferScreen
{
    public class CardLayout : MonoBehaviour
    {
        private float _width;
        [SerializeField] private int childrenBeforeSpacing;
        
        private GridLayoutGroup _gridLayoutGroup;

        private ReorderableListContent _reorderableListContent;

        private void Awake()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
            _width = GetComponent<RectTransform>().rect.width;
        }

        private void Start()
        {
            UpdateSpacing();
        }

        public void OnTransformChildrenChanged()
        {
            UpdateSpacing();
        }

        private void UpdateSpacing()
        {
            var childCount = transform.childCount;

            if (childCount > childrenBeforeSpacing)
            {
                var totalOverflow = _width - (230 * childCount);
                var spacing = (float) totalOverflow / (childCount - 1);
                _gridLayoutGroup.spacing = new Vector2(spacing, 0);
            }
            else
            {
                _gridLayoutGroup.spacing = new Vector2(0, 0);
            }
        }
    }
}