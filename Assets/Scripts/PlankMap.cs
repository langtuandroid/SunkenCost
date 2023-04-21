using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
using Etchings;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlankMap : MonoBehaviour
{
    public static PlankMap Current;
    
    [SerializeField] private GameObject _plankPrefab;
    [SerializeField] private Transform _plankGrid;
    
    private UnityEngine.UI.Extensions.ReorderableListNoEdges _plankGridController;
    
    private List<Plank> _planks = new List<Plank>();
    
    public int PlankCount { get; private set; }
    
    public Plank MouseOver => _planks.FirstOrDefault(plank => plank.MouseIsOver);

    private void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;
    }

    public void Start()
    {
        _plankGridController = _plankGrid.parent.GetComponent<UnityEngine.UI.Extensions.ReorderableListNoEdges>();
    }

    private void Update()
    {
        // Can't move or add if it's not the player's turn
        if (Battle.Current.GameState != GameState.PlayerTurn || Player.Current.IsOutOfMoves)
        {
            _plankGridController.IsDraggable = false;
        }
        else if (!_plankGridController.IsDraggable && !Player.Current.IsOutOfMoves)
        {
            _plankGridController.IsDraggable = true;
        }
    }

    public bool IsDragging => _plankGridController.IsDragging;

    public Plank GetPlank(int plankPosition)
    {
        return _plankGridController.Content.GetChild(plankPosition).GetComponent<Plank>();
    }

    public Plank CreatePlank()
    {
        var newPlank = Instantiate(_plankPrefab, _plankGrid);
        PlankCount++;
        newPlank.transform.SetSiblingIndex(PlankCount);
        _plankGridController.Refresh();

        var plank = newPlank.GetComponent<Plank>();
        _planks.Add(plank);
        return plank;
    }

    public BattleEvent DestroyPlank(DamageSource source, int plankPosition = -1, Plank plank = null)
    {
        if (plankPosition != -1)
            plank = GetPlank(plankPosition);

        if (plank is null)
        {
            throw new Exception("Trying to destroy plank that isn't there!");
        }

        _planks.Remove(plank);
        plank.DestroyPlank(source);
        if (plank.Etching) EtchingMap.Current.RemoveEtching(plank.Etching);
        PlankCount--;
        
        return new BattleEvent(BattleEventType.PlankDestroyed);
    }
    
    private IEnumerator WaitForPlankDestruction(Plank plank)
    {
        yield return 0;
        _plankGridController.Refresh();
    }

    public void RandomisePlanks()
    {
        _planks = _planks.OrderBy(s => Random.value).ToList();

        for (var i = 0; i < _planks.Count; i++)
        {
            _planks[i].transform.SetSiblingIndex(i+1);
        }
    }
}