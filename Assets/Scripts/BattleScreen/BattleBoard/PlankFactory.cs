using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Etchings;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlankFactory : MonoBehaviour
{
    public static PlankFactory Current;
    
    [SerializeField] private GameObject _plankPrefab;
    [SerializeField] private Transform _plankGrid;
    
    private Board _board;

    private void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;
    }

    public void Start()
    {
        _board = _plankGrid.parent.GetComponent<Board>();
    }
    

    public Plank CreatePlank()
    {
        var newPlank = Instantiate(_plankPrefab, _plankGrid);
        var plankRectTransform = newPlank.GetComponent<RectTransform>();
        plankRectTransform.SetAsLastSibling();

        return newPlank.GetComponent<Plank>();
    }
}