using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetHeart(bool hasHeart)
    {
        _image.sprite = hasHeart ? fullSprite : emptySprite;
    }
}
