using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
    public static MoveText current;
    private TextMeshProUGUI _text;
    private Color _defaultColor;

    private void Awake()
    {
        // One instance of static objects only
        if (current)
        {
            Destroy(gameObject);
            return;
        }
        
        current = this;
    }

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _defaultColor = _text.color;
    }

    public void NotEnoughMoves()
    {
        StartCoroutine(MakeRed());
    }

    private IEnumerator MakeRed()
    {
        _text.color = Color.red;
        yield return new WaitForSeconds(0.8f);
        _text.color = _defaultColor;
    }
}
