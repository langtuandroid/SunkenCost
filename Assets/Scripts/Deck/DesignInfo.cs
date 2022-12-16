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
        if (design.Level == 1) titleText.text += " +";
        else if (design.Level == 2) titleText.text += " X";
        
        descriptionText.text = DesignManager.GetDescription(design.Category, design.Stats);
        
        usesText.text = design.Limitless ? "" : design.Stats[St.UsesPerTurn].Value - design.UsesUsedThisTurn + "/" + design.Stats[St.UsesPerTurn].Value;
    }
}
