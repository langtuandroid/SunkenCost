using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemShaderDisplay : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private Material _material;
        private Transform _transform;
        
        private void Awake()
        {
            _material = _image.material;
            _transform = transform;
        }

        public void Activate()
        {
            StartCoroutine(ActivateCoroutine());
        }

        private IEnumerator ActivateCoroutine()
        {
            var localScale = _transform.localScale;
            var localPosition = _transform.localPosition;
            
            _material.EnableKeyword("OUTBASE_ON");
            _material.SetFloat("_OutlineAlpha", 0f);
            var progress = 0f;

            while (progress < 1f)
            {
                _material.SetFloat("_OutlineAlpha", progress);
                var vectorVals = (progress / 3) + localScale.x;
                _transform.localPosition = new Vector3(localPosition.x, localPosition.y + progress * -50);
                _transform.localScale = new Vector3(vectorVals, vectorVals, 1);

                progress += 0.05f;
                yield return new WaitForSecondsRealtime(0.001f);
            }
            
            yield return new WaitForSecondsRealtime(0.1f);
            
            while (progress > 0)
            {
                _material.SetFloat("_OutlineAlpha", progress);
                var vectorVals = (progress / 3) + localScale.x;
                _transform.localPosition = new Vector3(localPosition.x, localPosition.y + progress * -50);
                _transform.localScale = new Vector3(vectorVals, vectorVals, 1);

                progress -= 0.2f;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
            _transform.localScale = localScale;
            _transform.localPosition = localPosition;
            _material.DisableKeyword("OUTBASE_ON");
        }
    }
}