using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    private TextMeshProUGUI _headerText;
    private TextMeshProUGUI _contentText;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private RectTransform _rectTransform;
    private Image _image;
    private GameObject _header;
    private GameObject _content;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _header = transform.GetChild(0).gameObject;
        _content = transform.GetChild(1).gameObject;
        _headerText = _header.GetComponent<TextMeshProUGUI>();
        _contentText = _content.GetComponent<TextMeshProUGUI>();

        _image.enabled = false;
        _header.SetActive(false);
        _content.SetActive(false);
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;

        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        
        float finalPivotX = 0f;
        float finalPivotY = 0f;


    
        if (pivotX < 0.8) //If mouse on left of screen move tooltip to right of cursor and vice vera
        {
            finalPivotX = -0.1f;
        }
       
        else
        {
            finalPivotX = 1.01f;
        }

       

        if (pivotY < 0.2) //If mouse on lower bit of screen move tooltip above cursor and vice versa
        {
            finalPivotY = 0;
        }
       
        else
        {
            finalPivotY = 1;
        }

       
        _rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);


        transform.position = position;

    }

    public void SetActive(bool active)
    {
        StartCoroutine(Activate(active));
    }

    private IEnumerator Activate(bool active)
    {
        yield return 0;
        _image.enabled = active;
        _header.SetActive(active);
        _content.SetActive(active);
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            _headerText.gameObject.SetActive(false);
        }
        else
        {
            _headerText.gameObject.SetActive(true);
            _headerText.text = header;
        }

        _contentText.text = content;
        
        int headerLength = _headerText.text.Length;
        int contentLength = _contentText.text.Length;

        layoutElement.enabled =
            (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }
}
