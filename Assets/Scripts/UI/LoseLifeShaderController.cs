using System.Collections;
using System.Collections.Generic;
using AllIn1SpriteShader;
using UnityEngine;
using UnityEngine.UI;

public class LoseLifeShaderController : MonoBehaviour
{
    private Image _image;
    private Material _material;
    
    private void Start()
    {
        BattleEvents.Current.OnPlayerLostLife += PlayerLostLife;
        _image = GetComponent<Image>();
        _image.enabled = false;
        _material = _image.material;
    }

    private void PlayerLostLife()
    {
        StartCoroutine(ShowHeart());
    }

    private IEnumerator ShowHeart()
    {
        _image.enabled = true;

        var progress = 0f;

        while (progress < 0.7f)
        {
            _material.SetFloat("_Alpha", progress);
            var vectorVals = progress * 40 + 1f;
            transform.localScale = new Vector3(vectorVals, vectorVals, 1);

            progress += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        progress = 0f;
        while (progress < 1f)
        {
            _material.SetFloat("_FadeAmount", progress);
            progress += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        _image.enabled = false;
        transform.localScale = new Vector3(1, 1, 1);
        _material.SetFloat("_Alpha", 0f);
        _material.SetFloat("_FadeAmount", 0f);
    }
}
