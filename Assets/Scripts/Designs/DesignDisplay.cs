using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Designs
{
    public readonly struct DesignDisplayState
    {
        public readonly string title;
        public readonly string description;
        public readonly string uses;

        public DesignDisplayState(string title, string description, string uses) =>
            (this.title, this.description, this.uses) = (title, description, uses);
    }
    
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
            UpdateDisplay(GetDisplayState());
            image.sprite = design.Sprite;
            _canvasGroup.alpha = 1;
        }

        public void UpdateDisplayWithCurrentState()
        {
            UpdateDisplay(GetDisplayState());
        }

        protected void UpdateDisplay(DesignDisplayState displayState)
        {
            titleText.text = displayState.title;
            descriptionText.text = displayState.description;
            usesText.text = displayState.uses;
        }

        protected DesignDisplayState GetDisplayState()
        {
            var title = design.Title;
            if (design.Level == 1) title += " +";
            else if (design.Level == 2) title += " X";
        
            var rawDescription = DesignManager.GetDescription(design);

            var descriptionWithSprites = rawDescription
                .Replace("damage", "<sprite=0>")
                .Replace("movement", "<sprite=1>");

            var defaultColor = ColorUtility.ToHtmlStringRGB(descriptionText.color);
            var allColor = ColorUtility.ToHtmlStringRGB(new Color(0.6f, 0.58f, 0.3f));

            var finalDescription = descriptionWithSprites
                .Replace("all", "<color=#" + allColor + ">all<color=#" + defaultColor + ">");

            var uses = design.Limitless ? "" : design.Stats[StatType.UsesPerTurn].Value - design.UsesUsedThisTurn + "/" + design.Stats[StatType.UsesPerTurn].Value;
            
            return new DesignDisplayState(title, finalDescription, uses);
        }
    }
}
