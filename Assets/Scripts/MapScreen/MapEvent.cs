using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapScreen
{

    public class MapEvent : MonoBehaviour
    {
        private Button _button;
        public Disturbance disturbance;

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private void Start()
        {
            var image = transform.GetChild(2).GetComponent<Image>();
            image.sprite = disturbance.sprite;
            titleText.text = disturbance.title;
            descriptionText.text = disturbance.description;
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
