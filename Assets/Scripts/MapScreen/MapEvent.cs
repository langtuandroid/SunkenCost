using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapScreen
{

    public class MapEvent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [SerializeField] private Image innerWash;
        [SerializeField] private Sprite blueInnerWash;
        [SerializeField] private Sprite redInnerWash;

        public Disturbance disturbance;

        public bool isElite;
        //private Button _button;
        

        private void Start()
        {
            var image = transform.GetChild(2).GetComponent<Image>();
            image.sprite = disturbance.sprite;
            titleText.text = disturbance.title;
            descriptionText.text = disturbance.description;

            innerWash.sprite = isElite ? redInnerWash : blueInnerWash;
        }
        
        public void NextBattle()
        {
            RunProgress.SelectNextBattle(disturbance.disturbanceType);
            MainManager.Current.LoadNextBattle();
        }

        public void UpdateDescription(string desc)
        {
            descriptionText.text = desc;
        }
    }
}
