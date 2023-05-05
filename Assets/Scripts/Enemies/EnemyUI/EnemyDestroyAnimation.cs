using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies.EnemyUI
{
    public class EnemyDestroyAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Material _material;

        private void Awake()
        {
            _material = new Material(_image.material);
            _image.material = _material;
        }

        public void StartAnimation()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _material.EnableKeyword("FADE_ON");
            _material.EnableKeyword("OUTBASE_ON");
            
            var progress = 0f;
            
            while (progress < 1f)
            {
                _material.SetFloat("_FadeAmount", progress);
                _material.SetFloat("_FadeBurnTransition", progress);
                _material.SetFloat("_OutlineAlpha", progress);
                progress += 0.05f;
                
                yield return new WaitForSecondsRealtime(0.001f);
            }
        }
    }
}