using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Etchings;
using UnityEngine;
using Random = UnityEngine.Random;

public class StickManager : MonoBehaviour
{
    public static StickManager current;

    public GameObject stickPrefab;
    
    private UnityEngine.UI.Extensions.ReorderableListNoEdges _stickGridController;
    public Transform stickGrid;
    
    [SerializeField] private List<Stick> _sticks = new List<Stick>();
    public int stickCount = 0;

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

    public void Start()
    {
        _stickGridController = stickGrid.parent.GetComponent<UnityEngine.UI.Extensions.ReorderableListNoEdges>();

        //_sticks.Add(_stickGridController.GetComponentInChildren<Stick>());
        if (stickCount > 1) return;
        
        /*
        var stick = CreateStick();
        EtchingManager.current.CreateEtching(stick.GetComponent<Stick>(), new Stab());
        stick = CreateStick();
        EtchingManager.current.CreateEtching(stick.GetComponent<Stick>(), new Slinger());
        stick = CreateStick();
        EtchingManager.current.CreateEtching(stick.GetComponent<Stick>(), new Impede());
        */
    }

    private void Update()
    {
        // Can't move or add if it's not the player's turn
        if (BattleManager.Current.gameState != GameState.PlayerTurn) // || (PlayerController.current.IsOutOfMoves))
        {
            _stickGridController.IsDraggable = false;
            return;
        }

        // ELse
        if (!_stickGridController.IsDraggable)
        {
            _stickGridController.IsDraggable = true;
        }

        // Can't add or change anything if the player is dragging
        if (_stickGridController.IsDragging)
        {
            return;
        }
    }

    public bool IsDragging => _stickGridController.IsDragging;

    public Stick GetStick(int stickPosition)
    {
        return _stickGridController.Content.GetChild(stickPosition).GetComponent<Stick>();
    }

    public GameObject CreateStick()
    {
        var newStick = Instantiate(stickPrefab, stickGrid);
        newStick.transform.SetSiblingIndex(stickCount);
        stickCount++;
        _stickGridController.Refresh();
        _sticks.Add(newStick.GetComponent<Stick>());
        OldBattleEvents.Current.StickAdded();
        return newStick;
    }

    public void DestroyStick(int stickPosition = -1, Stick stick = null)
    {
        if (stickPosition != -1)
            stick = GetStick(stickPosition);

        if (stick is null)
        {
            Debug.Log("what the fuck?");
            return;
        }

        _sticks.Remove(stick);
        if (stick.etching) EtchingManager.Current.RemoveEtching(stick.etching);
        stickCount--;
        stick.DestroyStick();
        
        StartCoroutine(WaitForStickDestruction());
    }
    
    private IEnumerator WaitForStickDestruction()
    {
        yield return 0;
        _stickGridController.Refresh();
        EtchingManager.Current.StickDestroyed();
    }

    public Stick MouseOver => _sticks.FirstOrDefault(stick => stick.mouseIsOver);

    public void RandomisePlanks()
    {
        _sticks = _sticks.OrderBy(s => Random.value).ToList();

        for (var i = 0; i < _sticks.Count; i++)
        {
            _sticks[i].transform.SetSiblingIndex(i+1);
        }
    }
}