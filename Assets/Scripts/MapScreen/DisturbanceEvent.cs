using Disturbances;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapScreen
{

    public class DisturbanceEvent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Image innerWash;
        [SerializeField] private Sprite blueInnerWash;
        [SerializeField] private Sprite redInnerWash;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image foregroundImage;

        public Disturbance Disturbance { get; private set; }

        public void Init(Disturbance disturbance)
        {
            Disturbance = disturbance;
        }
        
        private void Start()
        {
            backgroundImage.enabled = Disturbance.DisturbanceType == DisturbanceType.Card 
                                      || Disturbance.DisturbanceType == DisturbanceType.EliteCard;
            foregroundImage.sprite = Disturbance.GetSprite();
            titleText.text = Disturbance.GetTitle();
            descriptionText.text = Disturbance.GetDescription();

            innerWash.sprite = Disturbance.IsElite ? redInnerWash : blueInnerWash;
        }
        
        public void NextBattle()
        {
            RunProgress.Current.SelectNextBattle(Disturbance);
            MainManager.Current.LoadNextBattle();
        }

        public void UpdateDescription(string desc)
        {
            descriptionText.text = desc;
        }
    }
}
