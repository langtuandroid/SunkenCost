using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;
    
    [SerializeField] private TextMeshProUGUI _movesText;

    private List<Heart> _hearts = new List<Heart>();

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
        BattleEvents.Current.OnBeginPlayerTurn += UpdateMovesText;
        
        for (var i = 0; i < heartsParentTransform.childCount; i++)
        {
            _hearts.Add(heartsParentTransform.GetChild(i).GetComponent<Heart>());
        }
    }


    public void UpdateMovesText()
    {
        var movesLeft = (PlayerController.current.MovesPerTurn - PlayerController.current.MovesUsedThisTurn).ToString();
        _movesText.text = movesLeft + "/" + PlayerController.current.MovesPerTurn;
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
