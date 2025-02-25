﻿using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleBoard;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class BoardScaler : MonoBehaviour
{
    public static BoardScaler current;

    private float _plankScale = 1f;
    private Vector3 _targetScale;

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
        SetBoardScale(RunProgress.Current.PlayerStats.Deck.Count);
    }

    public void SetBoardScale(int plankCount)
    {
        _plankScale = plankCount <= 3 ? 1 : 1.025f - (plankCount - 3) * 0.1f;
        transform.localScale =  new Vector3(_plankScale, _plankScale, 1);
    }
}
