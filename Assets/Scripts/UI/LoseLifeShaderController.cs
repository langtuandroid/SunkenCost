using System.Collections;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoseLifeShaderController : MonoBehaviour
    {
        [SerializeField] private BattleActivationShaderDisplay _battleActivationShaderDisplay;
    
        private Image _image;
        private Material _material;

        private void Start()
        {
            _image = GetComponent<Image>();
            _image.enabled = false;
            _material = _image.material;
        }

        public void PlayerLostLife()
        {
            StartCoroutine(ShowHeart());
        }

        private IEnumerator ShowHeart()
        {
            _image.enabled = true;
            _battleActivationShaderDisplay.Activate();
        
            _material.EnableKeyword("FADE_ON");
            _material.EnableKeyword("HITEFFECT_ON");
            
            _material.SetFloat("_Alpha", 1f);
            _material.SetFloat("_FadeAmount", 0f);

            var progress = 0f;
            
            while (progress < 1f)
            {
                _material.SetFloat("_FadeAmount", progress);
                progress += 0.02f;
                yield return new WaitForSecondsRealtime(0.01f);
            }

            _image.enabled = false;
            _material.DisableKeyword("FADE_ON");
            _material.DisableKeyword("HITEFFECT_ON");
        }
    }
}
