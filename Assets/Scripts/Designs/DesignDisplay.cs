using System;
using System.Collections;
using System.Collections.Generic;
using Designs;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DesignDisplay : MonoBehaviour
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
        image.sprite = design.Sprite;
        _canvasGroup.alpha = 1;
    }

    public void Refresh()
    {
        titleText.text = design.Title;
        if (design.Level == 1) titleText.text += " +";
        else if (design.Level == 2) titleText.text += " X";
        
        var description = DesignManager.GetDescription(design);

        var descriptionWithSprites = description
            .Replace("damage", "<sprite=0>")
            .Replace("movement", "<sprite=1>");

        var defaultColor = ColorUtility.ToHtmlStringRGB(descriptionText.color);
        var allColor = ColorUtility.ToHtmlStringRGB(new Color(0.6f, 0.58f, 0.3f));

        var descriptionWithColor = descriptionWithSprites
            .Replace("all", "<color=#" + allColor + ">all<color=#" + defaultColor + ">");
        
        descriptionText.text = descriptionWithColor;

        usesText.text = design.Limitless ? "" : design.Stats[StatType.UsesPerTurn].Value - design.UsesUsedThisTurn + "/" + design.Stats[StatType.UsesPerTurn].Value;
    }
}
