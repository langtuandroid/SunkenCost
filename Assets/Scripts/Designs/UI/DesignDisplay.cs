using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Designs.UI
{
    public class DesignDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        public TextMeshProUGUI TitleText => titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI usesText;
        [SerializeField] private Image image;
        [SerializeField] private DesignCategoryDisplay _categoryDisplay;
        [SerializeField] private DesignDamageAndRangeDisplay _damageAndRangeDisplay;
        
        public Design design;
        private CanvasGroup _canvasGroup;

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }

        protected virtual void Start()
        {
            // Give it one frame to catch up
            StartCoroutine(StartInitialisation());
        }

        private IEnumerator StartInitialisation()
        {
            yield return 0;
            Init();
        }

        protected virtual void Init()
        {
            TitleText.text = design.Title;
            _categoryDisplay.Init(design.Category);

            // Remove the damage / range area from the design if not applicable
            if (design.Category == DesignCategory.Effect)
            {
                _damageAndRangeDisplay.gameObject.SetActive(false);
                var descRectTransform = descriptionText.GetComponent<RectTransform>();
                var anchoredPosition = descRectTransform.anchoredPosition;
                descRectTransform.anchoredPosition = new Vector3(anchoredPosition.x, anchoredPosition.y + 25, 1);
            }

            image.sprite = design.Sprite;
            _canvasGroup.alpha = 1;
            
            // Allows the tooltipTrigger to be triggered
            _canvasGroup.enabled = false;
            
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            titleText.text = design.Title;
            if (design.Level == 1) titleText.text += " +";
            else if (design.Level == 2) titleText.text += " X";
            
            var rawDescription = DesignLoader.GetDescription(design);

            

            var defaultColor = ColorUtility.ToHtmlStringRGB(descriptionText.color);
            var allColor = ColorUtility.ToHtmlStringRGB(new Color(0.6f, 0.58f, 0.3f));

            descriptionText.text = rawDescription
                .Replace("all", "<color=#" + allColor + ">all<color=#" + defaultColor + ">");
            
            usesText.text = design.Limitless ? "" : design.GetStat(StatType.UsesPerTurn) - design.UsesUsedThisTurn 
                                                    + "/" + design.GetStat(StatType.UsesPerTurn);

            if (design.Category != DesignCategory.Effect)
                _damageAndRangeDisplay.RefreshInfo(design);
        }
    }
}
