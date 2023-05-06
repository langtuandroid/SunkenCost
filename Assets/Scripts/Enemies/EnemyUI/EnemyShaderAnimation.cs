using System;
using System.Collections;
using BattleScreen;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyShaderAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Material _material;

        private void Awake()
        {
            _material = new Material(_image.material);
            _image.material = _material;
        }
        
        public void StartDeathAnimation()
        {
            StartCoroutine(DeathAnimation());
        }
        

        private IEnumerator DeathAnimation()
        {
            _material.EnableKeyword("FADE_ON");
            _material.EnableKeyword("OUTBASE_ON");
            
            var progress = 0f;
            
            _material.SetFloat("_Glow", 1f);
            _material.EnableKeyword("GLOW_ON");

            while (progress < 1f)
            {
                _material.SetFloat("_FadeAmount", progress);
                _material.SetFloat("_FadeBurnTransition", progress);
                _material.SetFloat("_OutlineAlpha",  progress + 0.5f);
                progress += 0.05f;
                
                yield return new WaitForSecondsRealtime(0.001f);
            }
        }
    }
}