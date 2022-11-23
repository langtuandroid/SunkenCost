using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnFrame : MonoBehaviour
{
    private Image _image;
    private Material _material;
    
    private void Start()
    {
        GameEvents.current.OnEndPlayerTurn += OnEndPlayerTurn;
        GameEvents.current.OnEndEnemyTurn += OnEndEnemyTurn;
        _image = GetComponent<Image>();
        _material = _image.material;
        _material.SetFloat("_FadeAmount", 1f);
    }

    private void OnEndPlayerTurn()
    {
        StartCoroutine(EnemyTurn());
    }

    private void OnEndEnemyTurn()
    {
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        if (_material.GetFloat("_FadeAmount") > 0.5f) yield break; 
        
        var progress = 0f;

        while (progress < 1f)
        {
            _material.SetFloat("_FadeAmount", progress);
            progress += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        _image.enabled = false;
    }

    private IEnumerator EnemyTurn()
    {

        var progress = 0f;

        while (progress < 1f)
        {
            _material.SetFloat("_FadeAmount", 1f-progress);
            progress += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        _image.enabled = true;
    }
}
