using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace OfferScreen
{
    public class CardLayout : MonoBehaviour
    {
        private GridLayoutGroup _gridLayoutGroup;

        private ReorderableListContent _reorderableListContent;

        private void Awake()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
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

            if (childCount > 4)
            {
                var totalOverflow = 1100 - (230 * childCount);
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