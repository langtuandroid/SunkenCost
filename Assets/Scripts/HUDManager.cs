using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;
    
    [SerializeField] private TextMeshProUGUI _movesText;
    [SerializeField] private TextMeshProUGUI _totalMovesText;
    [SerializeField] private TextMeshProUGUI _roundText;

    private List<Heart> _hearts = new List<Heart>();

    [SerializeField] private Image _xpFillBar;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private Transform heartsParentTransform;

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
        for (var i = 0; i < heartsParentTransform.childCount; i++)
        {
            _hearts.Add(heartsParentTransform.GetChild(i).GetComponent<Heart>());
        }
    }

    private void Update()
    {
        _movesText.text = (PlayerController.current.MovesPerTurn - PlayerController.current.MovesUsedThisTurn).ToString();
        _totalMovesText.text = PlayerController.current.MovesPerTurn.ToString();

        if (GameManager.current.Round < 16)
            _roundText.text = "TURNS LEFT: " + (5 - GameManager.current.Round);
        else
        {
            _roundText.text = "GOOD LUCK!";
        }
    }

    public void UpdateLives()
    {
        var lives = PlayerController.current.Lives;

        for (var i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].SetHeart(lives > i);
        }
    }
}
