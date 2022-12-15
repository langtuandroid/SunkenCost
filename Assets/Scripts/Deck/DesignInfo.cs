using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DesignInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    public TextMeshProUGUI TitleText => titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI usesText;
    [SerializeField] private Image image;

    private CanvasGroup _canvasGroup;

    public Design design;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    protected virtual void Start()
    {
        // Give it one frame to catch up
        StartCoroutine(StartInitialisation());
    }

    private IEnumerator StartInitialisation()
    {
        yield return 0;
        Init();
    }

    protected virtual void Init()
    {
        TitleText.text = design.Title;
        Refresh();
        image.sprite = DesignManager.GetEtchingSprite(design.Category);
        _canvasGroup.alpha = 1;
    }

    public void Refresh()
    {
        titleText.text = design.Title;
        if (design.Level > 0) titleText.text += " " + ToRoman(design.Level);
        
        descriptionText.text = DesignManager.GetDescription(design.Category, design.Stats);
        
        usesText.text = design.Limitless ? "" : design.Stats[St.UsesPerTurn].Value - design.UsesUsedThisTurn + "/" + design.Stats[St.UsesPerTurn].Value;
    }
    
    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 999");
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "ERR";
    }
}
