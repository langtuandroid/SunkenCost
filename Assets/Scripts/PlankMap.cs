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
        if (Battle.Current.GameState != GameState.PlayerTurn)
        {
            _plankGridController.IsDraggable = false;
        }
        else if (!_plankGridController.IsDraggable)
        {
            _plankGridController.IsDraggable = true;
        }
    }

    public bool IsDragging => _plankGridController.IsDragging;

    public Plank GetPlank(int plankPosition)
    {
        return _plankGridController.Content.GetChild(plankPosition).GetComponent<Plank>();
    }

    public GameObject CreateStick()
    {
        var newPlank = Instantiate(_plankPrefab, _plankGrid);
        newPlank.transform.SetSiblingIndex(PlankCount);
        PlankCount++;
        _plankGridController.Refresh();
        _planks.Add(newPlank.GetComponent<Plank>());
        return newPlank;
    }

    public List<BattleEvent> DestroyPlank(DamageSource source, int plankPosition = -1, Plank plank = null)
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
        
        var plankDestructionEvent = new BattleEvent(BattleEventType.PlankDestroyed);
        return BattleEventsManager.Current.GetEventAndResponsesList(plankDestructionEvent);
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