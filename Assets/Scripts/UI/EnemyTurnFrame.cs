using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnFrame : MonoBehaviour
{
    private Image _image;
    private Material _material;

    private bool _running;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _material = _image.material;
        _material.SetFloat("_FadeAmount", 1f);
    }

    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurn());
    }

    public void EndEnemyTurn()
    {
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        if (_running)
            StopCoroutine(EnemyTurn());

        _running = true;
        
        var progress = 0f;

        while (progress < 1f)
        {
            _material.SetFloat("_FadeAmount", progress);
            progress += 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        _image.enabled = false;

        _running = false;
    }

    private IEnumerator EnemyTurn()
    {
        if (_running)
            StopCoroutine(PlayerTurn());

        _running = true;
        _image.enabled = true;
        

        var progress = 0f;

        while (progress < 1f)
        {
            _material.SetFloat("_FadeAmount", 1f-progress);
            progress += 0.1f;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        
        _running = false;
    }
}
