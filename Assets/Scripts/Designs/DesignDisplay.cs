using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Designs
{
    public class DesignDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        public TextMeshProUGUI TitleText => titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI usesText;
        [SerializeField] private Image image;
        
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
            UpdateDisplay();
            image.sprite = design.Sprite;
            _canvasGroup.alpha = 1;
        }

        public void UpdateDisplay()
        {
            titleText.text = design.Title;
            if (design.Level == 1) titleText.text += " +";
            else if (design.Level == 2) titleText.text += " X";
            
            var rawDescription = DesignManager.GetDescription(design);

            var descriptionWithSprites = rawDescription
                .Replace("damage", "<sprite=0>")
                .Replace("movement", "<sprite=1>");

            var defaultColor = ColorUtility.ToHtmlStringRGB(descriptionText.color);
            var allColor = ColorUtility.ToHtmlStringRGB(new Color(0.6f, 0.58f, 0.3f));

            descriptionText.text = descriptionWithSprites
                .Replace("all", "<color=#" + allColor + ">all<color=#" + defaultColor + ">");
            
            usesText.text = design.Limitless ? "" : design.Stats[StatType.UsesPerTurn].Value - design.UsesUsedThisTurn + "/" + design.Stats[StatType.UsesPerTurn].Value;
        }
    }
}
